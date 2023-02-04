namespace TokenBasedAuthApplication.Core.DTOs;

public sealed record CreateUserDto(
    string UserName,
    string Email,
    string Password
);