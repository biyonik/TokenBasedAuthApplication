using Microsoft.AspNetCore.Identity;

namespace TokenBasedAuthApplication.Core.Entities;

public sealed class AppUser : IdentityUser<Guid>
{
    public string? City { get; set; }
}