using System.Diagnostics.CodeAnalysis;
using Couple.Client.Features.Done.States;
using Couple.Client.Features.Issue.States;
using Couple.Client.Features.Synchronizer;
using Couple.Client.ProgramHelper;
using Couple.Client.Shared;
using Couple.Client.Shared.Data;
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
        builder.RootComponents.AddApp(builder.HostEnvironment, builder.Configuration);
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services
            .AddTransient(_ => new HttpClient { BaseAddress = new(builder.Configuration[Constants.ApiPrefix]!) })
            .AddDbContextFactory<AppDbContext>(options => options.UseSqlite($"Filename={Constants.DatabaseFileName}"))
            .AddScoped<DbContextProvider>()
            .AddScoped<IssueStateContainer>()
            .AddScoped<DoneStateContainer>()
            .AddScoped<Synchronizer>()
            .AddScoped<ApiAuthorizationMessageHandler>()
            .AddInitializer(builder.HostEnvironment, builder.Configuration)
            .AddHttpClient(builder.HostEnvironment, builder.Configuration);

        if (!builder.HostEnvironment.IsDevelopment() 
            || (builder.HostEnvironment.IsDevelopment() 
                && builder.Configuration.GetValue<bool>(Constants.IsAuthEnabled)))
        {
            builder.Services
                .AddB2CAuthentication(builder.Configuration);
        }

        var host = builder.Build();
        await host.RunAsync();
    }
}
