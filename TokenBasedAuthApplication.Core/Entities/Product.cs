namespace TokenBasedAuthApplication.Core.Entities;

public sealed class Product
{
    public Product()
    {
    }

    public Product(Guid Id,
        string Name,
        decimal Price,
        int Stock,
        Guid UserId)
    {
        this.Id = Id;
        this.Name = Name;
        this.Price = Price;
        this.Stock = Stock;
        this.UserId = UserId;
    }

    public Guid Id { get; init; }
    public string Name { get; init; }
    public decimal Price { get; init; }
    public int Stock { get; init; }
    public Guid UserId { get; init; }

    public void Deconstruct(out Guid Id, out string Name, out decimal Price, out int Stock, out Guid UserId)
    {
        Id = this.Id;
        Name = this.Name;
        Price = this.Price;
        Stock = this.Stock;
        UserId = this.UserId;
    }
}