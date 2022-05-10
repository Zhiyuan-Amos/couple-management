namespace Couple.Client.Shared;

public class AddIcon
{
    [EditorRequired] [Parameter] public EventCallback OnClickCallback { get; init; }
}