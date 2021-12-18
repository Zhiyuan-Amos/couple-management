namespace Couple.Client.States;

public class Notifier
{
    public event Action OnChange;

    protected void NotifyStateChanged() => OnChange?.Invoke();
}
