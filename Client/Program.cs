using AzureStaticWebApps.Blazor.Authentication;
using Couple.Client.Model.Calendar;
using Couple.Client.Model.ToDo;
using Couple.Client.Profiles;
using Couple.Client.Services;
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
                .AddTransient(_ => new HttpClient { BaseAddress = new Uri(builder.Configuration["API_Prefix"] ?? builder.HostEnvironment.BaseAddress) })
                .AddSingleton<ToDoStateContainer>()
                .AddSingleton<SelectedCategoryStateContainer>()
                .AddSingleton<EventStateContainer>()
                .AddSingleton<Synchronizer>()
                .AddStaticWebAppsAuthentication()
                .AddAutoMapper(typeof(ToDoProfile), typeof(EventProfile))
                .AddOptions()
                .AddAuthorizationCore();

            var host = builder.Build();

            var toDoStateContainer = host.Services.GetRequiredService<ToDoStateContainer>();
            var selectedCategoryStateContainer = host.Services.GetRequiredService<SelectedCategoryStateContainer>();
            var js = host.Services.GetRequiredService<IJSRuntime>();

            var initToDosTask = js.InvokeAsync<IJSObjectReference>("import", "./ToDo.razor.js")
                .AsTask()
                .ContinueWith(moduleTask => moduleTask.Result.InvokeAsync<List<ToDoModel>>("getAll").AsTask())
                .Unwrap()
                .ContinueWith(toDosTask =>
                {
                    toDoStateContainer.ToDos = toDosTask.Result;
                    selectedCategoryStateContainer.Reset();
                });

            var eventStateContainer = host.Services.GetRequiredService<EventStateContainer>();

            var initEventsTask = js.InvokeAsync<IJSObjectReference>("import", "./Event.razor.js")
                .AsTask()
                .ContinueWith(moduleTask => moduleTask.Result.InvokeAsync<List<EventModel>>("getAll").AsTask())
                .Unwrap()
                .ContinueWith(eventsTask => eventStateContainer.SetEvents(eventsTask.Result));

            var synchronizer = host.Services.GetRequiredService<Synchronizer>();
            var initSynchronizerTask = synchronizer.Initialization;

            await Task.WhenAll(initToDosTask, initEventsTask, initSynchronizerTask);

            synchronizer.SynchronizeAsync(); // No await to prevent app from being blocked. It's not a big deal if it fails to execute successfully

            await host.RunAsync();
        }
    }
}
