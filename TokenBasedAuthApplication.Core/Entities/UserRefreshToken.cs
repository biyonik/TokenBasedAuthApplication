namespace TokenBasedAuthApplication.Core.Entities;

public sealed class UserRefreshToken
{
    public UserRefreshToken()
    {
    }

    public UserRefreshToken(Guid UserId,
        string Code,
        DateTime Expiration)
    {
        this.UserId = UserId;
        this.Code = Code;
        this.Expiration = Expiration;
    }

    public Guid UserId { get; init; }
    public string Code { get; init; }
    public DateTime Expiration { get; init; }

    public void Deconstruct(out Guid UserId, out string Code, out DateTime Expiration)
    {
        UserId = this.UserId;
        Code = this.Code;
        Expiration = this.Expiration;
    }
}