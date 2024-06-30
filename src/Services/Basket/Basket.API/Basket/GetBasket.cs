namespace Basket.API.Basket;

public record GetBasketResponse(ShoppingCart Cart);

public record GetBasketQuery(string userName) : IQuery<GetBasketResponse>;

public class GetBasketQueryHandler : IQueryHandler<GetBasketQuery, GetBasketResponse>
{
    public async Task<GetBasketResponse> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        return new GetBasketResponse(new ShoppingCart("jsevere"));
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
        })
        .WithName("GetBasketByUserName")
        .Produces<GetBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Basket By UserName")
        .WithDescription("Get Basket By UserName");
    }
}
