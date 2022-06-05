using Couple.Client.Features.Calendar.States;
using Couple.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Couple.Client.Features.Calendar.Components;

public partial class EventCreateUpdateForm
{
    [EditorRequired][Parameter] public Func<Task> OnSaveCallback { get; set; } = default!;

    [CascadingParameter(Name = "CreateUpdateEventStateContainer")]
    private CreateUpdateEventStateContainer CreateUpdateEventStateContainer { get; init; } = default!;

    private bool IsSaveEnabled => CreateUpdateEventStateContainer.Title.Any();

    private void OnForChange(For @for) => CreateUpdateEventStateContainer.For = @for;

    private void Save() => OnSaveCallback();
}
