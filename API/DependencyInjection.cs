using System.Text;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using Persistence;

namespace API;

public static class DependencyInjection
{
    public static IServiceCollection AddWebAPI(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentityCore<AppUser>()
            .AddRoles<IdentityRole>().AddEntityFrameworkStores<DataContext>();
        builder.Services.AddIdentityServices(builder.Configuration);

        AddLogs(builder);
        
        return builder.Services;
    }

    private static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        var builder = services.AddIdentityCore<AppUser>(options =>
        {
            options.SignIn.RequireConfirmedEmail = true;
            options.User.RequireUniqueEmail = true;
        });

        builder = new IdentityBuilder(builder.UserType, builder.Services);

        builder.AddSignInManager<SignInManager<AppUser>>();

        var tokenValidationParams = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("Token:TokenSecret").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };

        services.AddSingleton(tokenValidationParams);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = tokenValidationParams;
                }
            );
            
        return services;
    }

    private static void AddLogs(WebApplicationBuilder builder)
    {
        builder.Logging
            .ClearProviders()
            .SetMinimumLevel(LogLevel.Trace)
            .AddConsole();
        builder.Host.UseNLog();
    }
    
}