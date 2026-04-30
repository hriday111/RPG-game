namespace RpgGame.Logger;

/// <summary>
/// Global logging facade so any module can record events.
/// </summary>
public static class GameLog
{
    private static ILogSink sink = new InMemoryLogSink();
    private static ILogEventFormatter formatter = new DefaultLogEventFormatter();

    /// <summary>
    /// Replaces the active sink implementation.
    /// </summary>
    /// <param name="newSink">New sink instance.</param>
    public static void UseSink(ILogSink newSink)
    {
        sink = newSink ?? throw new ArgumentNullException(nameof(newSink));
    }

    /// <summary>
    /// Replaces the event formatter strategy.
    /// </summary>
    /// <param name="newFormatter">New formatter instance.</param>
    public static void UseFormatter(ILogEventFormatter newFormatter)
    {
        formatter = newFormatter ?? throw new ArgumentNullException(nameof(newFormatter));
    }

    /// <summary>
    /// Writes a message to the active sink.
    /// </summary>
    public static void Write(string message) => sink.Log(message);
    /// <summary>
    /// Writes a concrete entry to the active sink.
    /// </summary>
    public static void Write(LogEntry entry) => sink.Log(entry);

    /// <summary>
    /// Writes a structured event using the active formatter strategy.
    /// </summary>
    /// <param name="evt">Structured event payload.</param>
    public static void Write(GameLogEvent evt)
    {
        ArgumentNullException.ThrowIfNull(evt);
        sink.Log(new LogEntry(formatter.Format(evt), evt.Timestamp));
    }

    /// <summary>
    /// Gets recent entries from the active sink.
    /// </summary>
    public static IReadOnlyList<LogEntry> Recent(int count) => sink.GetRecent(count);

    /// <summary>
    /// Gets all entries from the active sink.
    /// </summary>
    public static IReadOnlyList<LogEntry> All() => sink.GetAll();

    /// <summary>
    /// Flushes pending entries in the active sink.
    /// </summary>
    public static void Flush() => sink.Flush();
}
