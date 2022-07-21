using Couple.Client.Shared;
using Couple.Client.Shared.Options;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Couple.Client.ProgramHelper;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddInitializer(this IServiceCollection services, 
        IWebAssemblyHostEnvironment environment,
        WebAssemblyHostConfiguration configuration)
    {
        if (environment.IsDevelopment() && !configuration.GetValue<bool>(Constants.IsAuthEnabled))
        {
            services.AddScoped<Initializer, DevelopmentInitializer>();
        }
        else
        {
            services.AddScoped<Initializer, ProductionInitializer>();
        }

        return services;
    } 
    
    public static IServiceCollection AddHttpClient(this IServiceCollection services, 
        IWebAssemblyHostEnvironment environment,
        WebAssemblyHostConfiguration configuration)
    {
        const string httpClientName = "Api";
        var httpClientBuilder = services.AddHttpClient(httpClientName,
            client => client.BaseAddress = new(configuration[Constants.ApiPrefix]!));

        if (!environment.IsDevelopment()
            || (environment.IsDevelopment() && configuration.GetValue<bool>(Constants.IsAuthEnabled)))
        {
            httpClientBuilder.AddHttpMessageHandler<ApiAuthorizationMessageHandler>();
        }

        services.AddScoped(sp =>
            sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient(httpClientName));

        return services;
    } 
    
    public static IServiceCollection AddB2CAuthentication(this IServiceCollection services, 
        WebAssemblyHostConfiguration configuration)
    {
        services.AddMsalAuthentication(options =>
        {
            configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
            options.ProviderOptions.DefaultAccessTokenScopes.Add(Constants.Scope);
        });

        services.Configure<AuthenticationOptions>(configuration.GetSection("AzureAdB2C"));

        return services;
    }  
}
