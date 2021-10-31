using System.Net.Http;
using System.Threading.Tasks;
using Couple.Client.Services.Synchronizer;
using Couple.Client.States.Calendar;
using Couple.Client.States.Issue;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Couple.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services
                .AddTransient(_ => new HttpClient
                { BaseAddress = new(builder.Configuration["API_Prefix"] ?? builder.HostEnvironment.BaseAddress) })
                .AddSingleton<IssueStateContainer>()
                .AddSingleton<EventStateContainer>()
                .AddSingleton<SelectedDateStateContainer>()
                .AddSingleton<Synchronizer>();

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
}
