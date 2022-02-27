using Couple.Client.Data;
using Couple.Client.Services.Synchronizer;
using Couple.Client.States.Calendar;
using Couple.Client.States.Done;
using Couple.Client.States.Issue;
using Couple.Client.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Couple.Client;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services
            .AddTransient(_ => new HttpClient
            {
                BaseAddress = new(builder.Configuration["API_Prefix"] ?? builder.HostEnvironment.BaseAddress)
            })
            .AddDbContextFactory<AppDbContext>(options => options.UseSqlite($"Filename={Constants.DatabaseFileName}"))
            .AddScoped<DbContextProvider>()
            .AddScoped<IssueStateContainer>()
            .AddScoped<DoneStateContainer>()
            .AddScoped<EventStateContainer>()
            .AddScoped<SelectedDateStateContainer>()
            .AddScoped<Synchronizer>();

        var host = builder.Build();

        var httpClient = host.Services.GetRequiredService<HttpClient>();

        if (builder.HostEnvironment.IsStaging() || builder.HostEnvironment.IsProduction())
        {
            try
            {
                await httpClient.GetAsync("api/Ping");
            }
            catch (HttpRequestException)
            {
                var navigationManager = host.Services.GetRequiredService<NavigationManager>();
                navigationManager.NavigateTo("/login", true);
                return;
            }
        }

        await host.RunAsync();
    }
}
