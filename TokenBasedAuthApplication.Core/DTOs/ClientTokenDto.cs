namespace TokenBasedAuthApplication.Core.DTOs;

public sealed record ClientTokenDto(
    string RefreshToken,
    DateTime RefreshTokenExpiration
);