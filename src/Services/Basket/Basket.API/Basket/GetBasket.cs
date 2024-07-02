using Basket.API.Data;

namespace Basket.API.Basket;

public record GetBasketResponse(ShoppingCart? Cart);

public record GetBasketQuery(string UserName) : IQuery<GetBasketResponse>;

public class GetBasketQueryHandler(IBasketRepository repository) : IQueryHandler<GetBasketQuery, GetBasketResponse>
{
    public async Task<GetBasketResponse> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasketAsync(query.UserName, cancellationToken);

        return new GetBasketResponse(basket);
    }
}

public class GetBasketEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{userName}", async (string userName, ISender sender) =>
        {
            var response = await sender.Send(new GetBasketQuery(userName));

            return Results.Ok(response);

            //return response.Cart is null
            //    ? Results.NotFound(response)
            //    : Results.Ok(response);
        })
        .WithName("GetBasketByUserName")
        .Produces<GetBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Basket By UserName")
        .WithDescription("Get Basket By UserName");
    }
}
