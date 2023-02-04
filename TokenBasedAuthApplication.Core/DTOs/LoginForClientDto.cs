namespace TokenBasedAuthApplication.Core.DTOs;

public sealed record LoginForClientDto(
    string ClientId,
    string ClientSecret
);