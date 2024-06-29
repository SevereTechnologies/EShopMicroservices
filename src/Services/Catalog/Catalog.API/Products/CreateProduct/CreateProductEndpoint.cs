namespace Catalog.API.Products.CreateProduct;

public record CreateProductRequest : ICommand<CreateProductResponse>
{
    public string Name { get; set; } = default!;
    public List<string> Category { get; set; } = new();
    public string Description { get; set; } = default!;
    public string ImageName { get; set; } = default!;
    public decimal Price { get; set; }
}

public record CreateProductResponse
{
    public Guid Id { get; set; }
}

/// <summary>
/// The create product endpoint.
/// </summary>
public class CreateProductEndpoint : ICarterModule
{
    /// <summary>
    /// Add the routes.
    /// </summary>
    /// <param name="app">The app.</param>
    /// <exception cref="NotImplementedException"></exception>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateProductCommand>(); // map request to command

            var result = await sender.Send(command);

            var response = result.Adapt<CreateProductResponse>(); // map result from handler to response

            return Results.Created($"/products/{response.Id}", response);
        })
            .WithName("CreateProduct")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Product Summary")
            .WithDescription("Create Product Desc");
    }
}
