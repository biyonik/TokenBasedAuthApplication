namespace TokenBasedAuthApplication.Core.DTOs;

public sealed record ProductDto
(
    string Name,
    decimal Price,
    int Stock,
    Guid UserId
);