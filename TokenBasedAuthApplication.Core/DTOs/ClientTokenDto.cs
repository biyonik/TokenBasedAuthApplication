namespace TokenBasedAuthApplication.Core.DTOs;

public record ClientTokenDto(
    string RefreshToken,
    DateTime RefreshTokenExpiration
);