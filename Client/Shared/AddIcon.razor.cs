using Microsoft.AspNetCore.Components;

namespace Couple.Client.Shared;

public partial class AddIcon
{
    [EditorRequired][Parameter] public EventCallback OnClickCallback { get; init; }
}
