

using Microsoft.IdentityModel.Tokens;

namespace TokenBasedAuthApplication.API.Extensions;

public static class ServiceRegistrationExtension
{
    public static IServiceCollection AddManuelService(this IServiceCollection services, IConfiguration configuration)
    {
        GetServiceConfigures(services, configuration);

        MainServices(services);

        GetProjectServices(services);

        GetProjectRepositories(services);
        
        GetAuthenticationConfiguration(services, configuration);

        return services;
    }

    private static void GetProjectRepositories(IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    }

    private static void GetProjectServices(IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));
    }

    private static void MainServices(IServiceCollection services)
    {
        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddCors(setupAction =>
        {
            setupAction.AddPolicy("All",
                policyBuilder =>
                {
                    policyBuilder.AllowAnyHeader().AllowCredentials().AllowAnyMethod().AllowAnyOrigin();
                });
        });
        services.AddDbContext<AppDbContext>();
        services.AddIdentity<AppUser, IdentityRole<Guid>>((IdentityOptions identityOptions) =>
            {
                identityOptions.User.RequireUniqueEmail = true;
                identityOptions.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
    }

    private static void GetServiceConfigures(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CustomTokenOptions>(configuration.GetSection("TokenOptions"));
        services.Configure<List<Client>>(configuration.GetSection("Clients"));
    }

    private static void GetAuthenticationConfiguration(IServiceCollection services, IConfiguration configuration)
    {
        var tokenOptions = configuration.GetSection("TokenOptions").Get<CustomTokenOptions>();
        
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
                IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });
    }
}