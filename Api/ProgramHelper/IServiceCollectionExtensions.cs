using Couple.Api.Shared.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;

namespace Couple.Api.ProgramHelper;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddUserService(this IServiceCollection services, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            services.AddScoped<IUserService, DevelopmentUserService>();
        }
        else
        {
            services.AddScoped<IUserService, ProductionUserService>();
        }

        return services;
    } 
    
    public static IServiceCollection AddCors(this IServiceCollection services, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            services.AddCors(options => options.AddDefaultPolicy(
                policy => policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()));
        }
        else
        {
            services.AddCors(options => options.AddDefaultPolicy(
                policy => policy.WithOrigins("https://couple.z23.web.core.windows.net")
                    .AllowAnyHeader()
                    .AllowAnyMethod()));
        }

        return services;
    }

    public static IServiceCollection AddDefaultAuthorization(this IServiceCollection services)
    {
        services
            .AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

        return services;
    }
    
    public static IServiceCollection AddB2CAuthentication(this IServiceCollection services, 
        ConfigurationManager configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(options =>
                {
                    configuration.Bind("AzureAdB2C", options);
                },
                options => configuration.Bind("AzureAdB2C", options));

        return services;
    }  
}
