namespace RpgGame.Logger;

/// <summary>
/// Contract for log storage backends used by the game.
/// </summary>
public interface ILogSink
{
    /// <summary>
    /// Adds a new log entry.
    /// </summary>
    /// <param name="entry">The entry to store.</param>
    void Log(LogEntry entry);

    /// <summary>
    /// Adds a message as a new log entry with current timestamp.
    /// </summary>
    /// <param name="message">Message to store.</param>
    void Log(string message);

    /// <summary>
    /// Returns the newest log entries up to <paramref name="count"/>.
    /// </summary>
    /// <param name="count">Maximum number of entries to return.</param>
    /// <returns>A read-only list ordered from oldest to newest within the selected tail.</returns>
    IReadOnlyList<LogEntry> GetRecent(int count);

    /// <summary>
    /// Returns all entries available in this sink.
    /// </summary>
    IReadOnlyList<LogEntry> GetAll();

    /// <summary>
    /// Persists any buffered data.
    /// </summary>
    void Flush();
}
