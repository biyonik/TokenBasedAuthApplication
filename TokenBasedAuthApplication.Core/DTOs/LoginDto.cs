namespace TokenBasedAuthApplication.Core.DTOs;

public sealed record LoginDto(
    string Email,
    string Password
);