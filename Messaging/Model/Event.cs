namespace Couple.Messaging.Model;

public class Event
{
    public string? Topic { get; init; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string? Subject { get; init; }
    public string? EventType { get; init; }
    public DateTime EventTime { get; init; }
    public IDictionary<string, object>? Data { get; init; }
}
