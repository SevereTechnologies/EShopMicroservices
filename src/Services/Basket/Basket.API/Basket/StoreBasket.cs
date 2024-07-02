
using Basket.API.Data;
using Discount.Grpc;

namespace Basket.API.Basket;

public record StoreBasketResponse(string UserName);

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResponse>;

public class StoreBasketValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
        RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

public class StoreBasketHandler(IBasketRepository repository, DiscountProtoService.DiscountProtoServiceClient discountProtoClient) : ICommandHandler<StoreBasketCommand, StoreBasketResponse>
{
    public async Task<StoreBasketResponse> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        // communicate with Discount.Grpc and get discount for product then calculate latest prices of 
        await DeductDiscount(command.Cart, cancellationToken);

        ShoppingCart cart = command.Cart;

        // Store basket in database
        await repository.StoreBasketAsync(cart, cancellationToken);

        // TODO: Store basket in cache

        return new StoreBasketResponse(command.Cart.UserName);
    }

    private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
    {
        // Communicate with Discount.Grpc and calculate lastest prices of products into sc
        foreach (var item in cart.Items)
        {
            var coupon = await discountProtoClient.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: cancellationToken);
            item.Price -= coupon.Amount;
        }
    }
}

public class StoreBasketEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", async (StoreBasketCommand command, ISender sender) =>
        {
            var response = await sender.Send(command);

            return Results.Created($"/basket/{response.UserName}", response);
        })
        .WithName("CreateBasket")
        .Produces<StoreBasketResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Basket")
        .WithDescription("Create Basket");
    }
}