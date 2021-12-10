using Microsoft.AspNetCore.Components;

namespace Couple.Client.Pages.Calendar.Components;

public partial class CategoryTreeViewTopBar
{
    [Parameter] public EventCallback OnCrossCallback { get; init; }

    [Parameter] public EventCallback OnSaveCallback { get; init; }
}
