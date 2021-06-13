using Couple.Client.Services;
using Couple.Client.States.Calendar;
using Couple.Client.States.ToDo;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading.Tasks;

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
                    {BaseAddress = new(builder.Configuration["API_Prefix"] ?? builder.HostEnvironment.BaseAddress)})
                .AddSingleton<ToDoStateContainer>()
                .AddSingleton<CreateUpdateToDoStateContainer>()
                .AddSingleton<EventStateContainer>()
                .AddSingleton<SelectedDateStateContainer>()
                .AddSingleton<Synchronizer>();

            var host = builder.Build();
            var httpClient = host.Services.GetRequiredService<HttpClient>();

            try
            {
                await httpClient.GetAsync("api/Ping");
            }
            catch (HttpRequestException hre)
            {
                var navigationManager = host.Services.GetRequiredService<NavigationManager>();
                navigationManager.NavigateTo("/login", true);
                return;
            }

            await host.RunAsync();
        }
    }
}
