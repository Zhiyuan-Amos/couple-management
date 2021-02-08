using AzureStaticWebApps.Blazor.Authentication;
using BlazorState;
using Couple.Client.Data;
using MediatR;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using static Couple.Client.States.Calendar.EventDataState;
using static Couple.Client.States.ToDo.SelectedCategoryState;
using static Couple.Client.States.ToDo.ToDoDataState;

namespace Couple.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddTelerikBlazor()
                .AddBlazorState((options) => options.Assemblies
                    = new Assembly[] { typeof(Program).GetTypeInfo().Assembly, })
                .AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["API_Prefix"] ?? builder.HostEnvironment.BaseAddress) })
                .AddScoped<LocalStore>()
                .AddStaticWebAppsAuthentication()
                .AddOptions()
                .AddAuthorizationCore();

            var host = builder.Build();

            var localStore = host.Services.GetRequiredService<LocalStore>();
            var mediator = host.Services.GetRequiredService<IMediator>();

            await mediator.Send(new RefreshToDosAction(localStore));
            await mediator.Send(new RefreshSelectedCategoryAction());
            await mediator.Send(new RefreshEventsAction(localStore));

            await host.RunAsync();
        }
    }
}
