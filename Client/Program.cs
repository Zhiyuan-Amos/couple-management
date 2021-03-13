using Couple.Client.Profiles;
using Couple.Client.Services;
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
                .AddTransient(_ => new HttpClient
                    {BaseAddress = new Uri(builder.Configuration["API_Prefix"] ?? builder.HostEnvironment.BaseAddress)})
                .AddSingleton<ToDoStateContainer>()
                .AddSingleton<SelectedCategoryStateContainer>()
                .AddSingleton<EventStateContainer>()
                .AddSingleton<Synchronizer>()
                .AddAutoMapper(typeof(ToDoProfile), typeof(EventProfile));

            var host = builder.Build();

            _ = host.Services.GetRequiredService<Synchronizer>();

            await host.RunAsync();
        }
    }
}
