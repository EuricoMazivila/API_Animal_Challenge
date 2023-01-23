using System.Text;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace API;

public static class DependencyInjection
{
    public static IServiceCollection AddWebAPI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityCore<AppUser>()
            .AddRoles<IdentityRole>().AddEntityFrameworkStores<DataContext>();
        services.AddIdentityServices(configuration);    
        
        return services;
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
    
}