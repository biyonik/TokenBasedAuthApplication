namespace TokenBasedAuthApplication.Core.DTOs;

public sealed record TokenDto (
    string AccessToken,
    DateTime AccessTokenExpiration,
    string RefreshToken,
    DateTime RefreshTokenExpiration
);