namespace TokenBasedAuthApplication.Core.DTOs;

public sealed record ClientTokenDto(
    string AccessToken,
    DateTime AccessTokenExpiration
);