
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

public class StoreBasketHandler : ICommandHandler<StoreBasketCommand, StoreBasketResponse>
{
    public async Task<StoreBasketResponse> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        ShoppingCart cart = command.Cart;

        // TODO: Store basket in database

        // TODO: Store basket in cache

        return new StoreBasketResponse("jsevere");
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
