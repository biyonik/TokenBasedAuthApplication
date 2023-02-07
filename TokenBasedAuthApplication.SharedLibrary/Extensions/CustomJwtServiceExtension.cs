using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TokenBasedAuthApplication.SharedLibrary.Authentication;


namespace TokenBasedAuthApplication.SharedLibrary.Extensions;

public static class CustomJwtServiceExtension
{
    public static void GetAuthenticationConfiguration(this IServiceCollection services, IConfiguration configuration, CustomTokenOptions? tokenOptions, SecurityKey securityKey)
    {
        services.AddAuthentication(configureOptions: (AuthenticationOptions options) =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, (JwtBearerOptions options) =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = tokenOptions.Issuer,
                ValidAudience = tokenOptions.Audience[0],
                IssuerSigningKey = securityKey,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });
    }
}