using Couple.Client.Features.Calendar.States;
using Couple.Client.Features.Synchronizer;
using Microsoft.AspNetCore.Components;

namespace Couple.Client.Features.Calendar;

public abstract class CreateUpdateEventBase : ComponentBase
{
    [Inject] protected HttpClient HttpClient { get; init; } = default!;

    [Inject] protected NavigationManager NavigationManager { get; init; } = default!;

    [Inject] protected EventStateContainer EventStateContainer { get; init; } = default!;

    [Inject] protected DbContextProvider DbContextProvider { get; init; } = default!;

    protected CreateUpdateEventStateContainer CreateUpdateEventStateContainer { get; set; } = default!;
    protected abstract Task Save();
}
