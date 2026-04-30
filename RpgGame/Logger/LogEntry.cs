namespace RpgGame.Logger;

/// <summary>
/// Represents one message in the game event log.
/// </summary>
public class LogEntry
{
    /// <summary>
    /// Gets the textual message for this event.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets the timestamp when the event was recorded.
    /// </summary>
    public DateTime Timestamp { get; }

    /// <summary>
    /// Initializes a new entry with current local time.
    /// </summary>
    /// <param name="message">Message text.</param>
    public LogEntry(string message)
        : this(message, DateTime.Now)
    {
    }

    /// <summary>
    /// Initializes a new entry with an explicit timestamp.
    /// </summary>
    /// <param name="message">Message text.</param>
    /// <param name="timestamp">Event timestamp.</param>
    public LogEntry(string message, DateTime timestamp)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
        Timestamp = timestamp;
    }

    /// <summary>
    /// Returns the display form used in journal and file logs.
    /// </summary>
    public override string ToString()
    {
        return $"{Timestamp:yyyy-MM-dd HH:mm:ss.fff}\t{Message}";
    }
}
