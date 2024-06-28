namespace Catalog.API.Products.CreateProduct;

/// <summary>
/// The create product command.
/// </summary>
public record CreateProductCommand : ICommand<CreateProductResult>
{
    public string Name { get; set; } = default!;
    public List<string> Category { get; set; } = new();
    public string Description { get; set; } = default!;
    public string ImageName { get; set; } = default!;
    public decimal Price { get; set; }
}

/// <summary>
/// The create product result.
/// </summary>
public record CreateProductResult
{
    public Guid Id { get; set; }
}

/// <summary>
/// The create product handler.
/// </summary>
public class CreateProductHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    /// <summary>
    /// Handle and return a task of type createproductresponse.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><![CDATA[Task<CreateProductResponse>]]></returns>
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageName = command.ImageName,
            Price = command.Price
        };

        // save to database
        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);

        // return result
        return new CreateProductResult { Id = product.Id };
    }
}
