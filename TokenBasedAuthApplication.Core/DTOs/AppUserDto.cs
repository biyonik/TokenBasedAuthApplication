namespace TokenBasedAuthApplication.Core.DTOs;

public sealed record AppUserDto(
    Guid Id,
    string UserName,
    string Email,
    string City
);