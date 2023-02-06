using Microsoft.AspNetCore.Identity;
using TokenBasedAuthApplication.Business.Services.Common;
using TokenBasedAuthApplication.Core.DataAccess.Common;
using TokenBasedAuthApplication.Core.Entities;
using TokenBasedAuthApplication.Core.Services.Common;
using TokenBasedAuthApplication.DataAccess.Contexts.EntityFramework;
using TokenBasedAuthApplication.DataAccess.Repositories.Common;

namespace TokenBasedAuthApplication.API.Extensions;

public static class ServiceRegistrationExtension
{
    public static IServiceCollection AddManuelService(this IServiceCollection services, IConfiguration configuration)
    {
        GetServiceConfigures(services, configuration);

        MainServices(services);

        GetProjectServices(services);

        GetProjectRepositories(services);

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
}