namespace TokenBasedAuthApplication.Core.DTOs;

public sealed record ProductDto
(
    Guid Id,
    string Name,
    decimal Price,
    int Stock,
    Guid UserId
);