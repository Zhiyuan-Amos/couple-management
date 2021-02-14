using AzureStaticWebApps.Blazor.Authentication;
using Couple.Client.Data.Calendar;
using Couple.Client.Data.ToDo;
using Couple.Client.States.Calendar;
using Couple.Client.States.ToDo;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
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
                .AddSingleton<ToDoStateContainer>()
                .AddSingleton<SelectedCategoryStateContainer>()
                .AddSingleton<EventStateContainer>()
                .AddStaticWebAppsAuthentication()
                .AddOptions()
                .AddAuthorizationCore();

            var host = builder.Build();

            var toDoStateContainer = host.Services.GetRequiredService<ToDoStateContainer>();
            var selectedCategoryStateContainer = host.Services.GetRequiredService<SelectedCategoryStateContainer>();
            var eventStateContainer = host.Services.GetRequiredService<EventStateContainer>();
            var js = host.Services.GetRequiredService<IJSRuntime>();

            var ToDoModule = await js.InvokeAsync<IJSObjectReference>("import", "./ToDo.razor.js");
            var EventModule = await js.InvokeAsync<IJSObjectReference>("import", "./Event.razor.js");

            var toDos = await ToDoModule.InvokeAsync<List<ToDoModel>>("getAll");
            toDoStateContainer.ToDos = toDos;
            selectedCategoryStateContainer.Reset();
            var events = await EventModule.InvokeAsync<List<EventModel>>("getAll");
            eventStateContainer.SetEvents(events);

            await host.RunAsync();
        }
    }
}
