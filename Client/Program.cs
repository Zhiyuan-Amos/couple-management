using AzureStaticWebApps.Blazor.Authentication;
using Couple.Client.Data;
using Couple.Client.States.Calendar;
using Couple.Client.States.ToDo;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
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

            builder.Services.AddTelerikBlazor()
                .AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["API_Prefix"] ?? builder.HostEnvironment.BaseAddress) })
                .AddSingleton<LocalStore>()
                .AddSingleton<ToDoStateContainer>()
                .AddSingleton<SelectedCategoryStateContainer>()
                .AddSingleton<EventStateContainer>()
                .AddStaticWebAppsAuthentication()
                .AddOptions()
                .AddAuthorizationCore();

            var host = builder.Build();

            var localStore = host.Services.GetRequiredService<LocalStore>();
            var toDoStateContainer = host.Services.GetRequiredService<ToDoStateContainer>();
            var selectedCategoryStateContainer = host.Services.GetRequiredService<SelectedCategoryStateContainer>();
            var eventStateContainer = host.Services.GetRequiredService<EventStateContainer>();

            await toDoStateContainer.RefreshAsync();
            selectedCategoryStateContainer.Reset();
            await eventStateContainer.RefreshAsync();

            await host.RunAsync();
        }
    }
}
