namespace TokenBasedAuthApplication.Core.Entities;

public sealed record Product(
    Guid Id,
    string Name,
    decimal Price,
    int Stock,
    Guid UserId
);