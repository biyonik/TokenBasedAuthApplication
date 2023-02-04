namespace TokenBasedAuthApplication.Core.Entities;

public sealed record UserRefreshToken(  
    Guid UserId,
    string Code,
    DateTime Expiration
);