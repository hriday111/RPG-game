namespace RpgGame.Logger;

/// <summary>
/// Represents a polymorphic logging event that can render itself as text.
/// </summary>
public interface ILogEvent
{
    /// <summary>
    /// Gets the timestamp associated with this event.
    /// </summary>
    DateTime Timestamp { get; }

    /// <summary>
    /// Creates the human-readable message for this event.
    /// </summary>
    /// <returns>Formatted log message.</returns>
    string ToMessage();
}
