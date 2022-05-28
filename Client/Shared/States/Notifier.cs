namespace Couple.Client.Shared.States;

public class Notifier
{
    public event Action? OnChange;

    protected void NotifyStateChanged() => OnChange?.Invoke();
}
