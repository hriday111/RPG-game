namespace RpgGame.Logger;

/// <summary>
/// Strategy contract for converting structured events into displayable messages.
/// </summary>
public interface ILogEventFormatter
{
    /// <summary>
    /// Formats a structured log event into a single text line.
    /// </summary>
    /// <param name="evt">Event to format.</param>
    /// <returns>Formatted message text.</returns>
    string Format(GameLogEvent evt);
}
