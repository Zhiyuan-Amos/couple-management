using System.Diagnostics.CodeAnalysis;
using Couple.Client.Data;
using Couple.Client.Infrastructure;
using Couple.Client.Services.Synchronizer;
using Couple.Client.States.Done;
using Couple.Client.States.Issue;
using Couple.Client.Utility;
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
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services
            .AddTransient(_ => new HttpClient { BaseAddress = new(builder.Configuration[Constants.ApiPrefix]!) })
            .AddDbContextFactory<AppDbContext>(options => options.UseSqlite($"Filename={Constants.DatabaseFileName}"))
            .AddScoped<DbContextProvider>()
            .AddScoped<IssueStateContainer>()
            .AddScoped<DoneStateContainer>()
            .AddScoped<Synchronizer>()
            .AddScoped<ApiAuthorizationMessageHandler>();

        const string httpClientName = "Api";
        builder.Services.AddHttpClient(httpClientName,
                client => client.BaseAddress = new(builder.Configuration[Constants.ApiPrefix]!))
            .AddHttpMessageHandler<ApiAuthorizationMessageHandler>();

        builder.Services.AddScoped(sp =>
            sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient(httpClientName));

        builder.Services.AddMsalAuthentication(options =>
        {
            builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);

            options.ProviderOptions.DefaultAccessTokenScopes.Add(ApiAuthorizationMessageHandler.Scope);
            options.ProviderOptions.DefaultAccessTokenScopes.Add("openid");
            options.ProviderOptions.DefaultAccessTokenScopes.Add("offline_access");

            options.ProviderOptions.LoginMode = "redirect";
        });

        var host = builder.Build();
        await host.RunAsync();
    }
}
