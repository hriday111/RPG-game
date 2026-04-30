namespace RpgGame.Logger;

/// <summary>
/// Represents a structured log event with a type and optional payload data.
/// </summary>
public sealed class GameLogEvent
{
    /// <summary>
    /// Gets the event category.
    /// </summary>
    public LogEventType Type { get; }

    /// <summary>
    /// Gets the event timestamp.
    /// </summary>
    public DateTime Timestamp { get; }

    /// <summary>
    /// Gets event payload values used by formatters.
    /// </summary>
    public IReadOnlyDictionary<string, object> Data { get; }

    /// <summary>
    /// Creates a new log event.
    /// </summary>
    /// <param name="type">Event category.</param>
    /// <param name="data">Optional payload values.</param>
    public GameLogEvent(LogEventType type, IDictionary<string, object>? data = null)
    {
        Type = type;
        Timestamp = DateTime.Now;
        Data = data is null
            ? new Dictionary<string, object>()
            : new Dictionary<string, object>(data);
    }
}
