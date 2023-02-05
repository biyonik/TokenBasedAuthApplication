namespace TokenBasedAuthApplication.Core.DTOs;

public class ProductDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public decimal Price { get; init; }
    public int Stock { get; init; }
    public Guid UserId { get; init; }
}