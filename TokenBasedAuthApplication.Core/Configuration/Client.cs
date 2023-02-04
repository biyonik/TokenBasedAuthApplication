namespace TokenBasedAuthApplication.Core.Configuration;

public sealed record Client(
    string Id,
    string Secret,
    List<string> Audiences
);