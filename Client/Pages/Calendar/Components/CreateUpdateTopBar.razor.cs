using Microsoft.AspNetCore.Components;

namespace Couple.Client.Pages.Calendar.Components;

public partial class CreateUpdateTopBar
{
    [Inject] private NavigationManager NavigationManager { get; init; }

    [Parameter] public EventCallback OnClickCallback { get; init; }

    [Parameter] public string Title { get; set; }

    [Parameter] public bool IsEnabled { get; set; }

    private void Cancel() => NavigationManager.NavigateTo("/calendar");
}
