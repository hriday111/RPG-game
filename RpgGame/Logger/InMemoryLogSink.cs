namespace RpgGame.Logger;

/// <summary>
/// Stores all log entries in memory for UI views (recent and full journal).
/// </summary>
public sealed class InMemoryLogSink : ILogSink
{
    private readonly List<LogEntry> entries = new();
    private readonly object gate = new();

    /// <inheritdoc />
    public void Log(LogEntry entry)
    {
        ArgumentNullException.ThrowIfNull(entry);
        lock (gate)
        {
            entries.Add(entry);
        }
    }

    /// <inheritdoc />
    public void Log(string message)
    {
        Log(new LogEntry(message));
    }

    /// <inheritdoc />
    public IReadOnlyList<LogEntry> GetRecent(int count)
    {
        if (count <= 0)
            return Array.Empty<LogEntry>();

        lock (gate)
        {
            int start = Math.Max(0, entries.Count - count);
            return entries.Skip(start).ToList();
        }
    }

    /// <inheritdoc />
    public IReadOnlyList<LogEntry> GetAll()
    {
        lock (gate)
        {
            return entries.ToList();
        }
    }

    /// <inheritdoc />
    public void Flush()
    {
        // No-op for pure in-memory storage.
    }
}
