using System.Diagnostics.CodeAnalysis;
using Couple.Client.Features.Done.States;
using Couple.Client.Features.Issue.States;
using Couple.Client.Features.Synchronizer;
using Couple.Client.ProgramHelper;
using Couple.Client.Shared;
using Couple.Client.Shared.Data;
using Couple.Client.Shared.Options;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Couple.Client;

public class Program
{
    /// <summary>
    ///     FIXME: This is required for EF Core 6.0 as it is not compatible with trimming.
    ///     See https://github.com/dotnet/efcore/issues/26288 & https://github.com/dotnet/efcore/issues/26860
    /// </summary>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
    // ReSharper disable once UnusedMember.Local
#pragma warning disable IDE0052
    private static readonly Type s_keepDateOnly = typeof(DateOnly);
#pragma warning restore IDE0052

    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        if (builder.HostEnvironment.IsDevelopment())
        {
            builder.RootComponents.Add<DevelopmentApp>("#app");
        }
        else
        {
            builder.RootComponents.Add<ProductionApp>("#app");
        }
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services
            .AddTransient(_ => new HttpClient { BaseAddress = new(builder.Configuration[Constants.ApiPrefix]!) })
            .AddDbContextFactory<AppDbContext>(options => options.UseSqlite($"Filename={Constants.DatabaseFileName}"))
            .AddScoped<DbContextProvider>()
            .AddScoped<IssueStateContainer>()
            .AddScoped<DoneStateContainer>()
            .AddScoped<Synchronizer>()
            .AddScoped<ApiAuthorizationMessageHandler>();
        
        if (builder.HostEnvironment.IsDevelopment())
        {
            builder.Services.AddScoped<Initializer, DevelopmentInitializer>();
        }
        else
        {
            builder.Services.AddScoped<Initializer, ProductionInitializer>();
        }

        const string httpClientName = "Api";
        var httpClientBuilder = builder.Services.AddHttpClient(httpClientName,
            client => client.BaseAddress = new(builder.Configuration[Constants.ApiPrefix]!));

        if (!builder.HostEnvironment.IsDevelopment())
        {
            httpClientBuilder.AddHttpMessageHandler<ApiAuthorizationMessageHandler>();

            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
                options.ProviderOptions.DefaultAccessTokenScopes.Add(Constants.Scope);
            });

            builder.Services.Configure<AuthenticationOptions>(builder.Configuration.GetSection("AzureAdB2C"));
        }

        builder.Services.AddScoped(sp =>
            sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient(httpClientName));

        var host = builder.Build();
        await host.RunAsync();
    }
}
