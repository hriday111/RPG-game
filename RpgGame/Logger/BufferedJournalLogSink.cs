namespace RpgGame.Logger;

/// <summary>
/// Keeps all entries in memory and periodically flushes pending items to file.
/// </summary>
public sealed class BufferedJournalLogSink : ILogSink
{
    private readonly InMemoryLogSink memorySink;
    private readonly FileLogSink fileSink;
    private readonly List<LogEntry> pending = new();
    private readonly object gate = new();
    private readonly int flushEveryNFrames;
    private int frameCounter;

    /// <summary>
    /// Gets the backing log file path.
    /// </summary>
    public string FilePath => fileSink.FilePath;

    /// <summary>
    /// Initializes a buffered sink using in-memory journal and periodic file flushing.
    /// </summary>
    /// <param name="memorySink">In-memory sink used for journal UI.</param>
    /// <param name="fileSink">Persistent sink used for file output.</param>
    /// <param name="flushEveryNFrames">Frame interval for flushing buffered entries.</param>
    public BufferedJournalLogSink(InMemoryLogSink memorySink, FileLogSink fileSink, int flushEveryNFrames)
    {
        this.memorySink = memorySink ?? throw new ArgumentNullException(nameof(memorySink));
        this.fileSink = fileSink ?? throw new ArgumentNullException(nameof(fileSink));
        if (flushEveryNFrames <= 0)
            throw new ArgumentOutOfRangeException(nameof(flushEveryNFrames), "Flush interval must be > 0.");
        this.flushEveryNFrames = flushEveryNFrames;
    }

    /// <inheritdoc />
    public void Log(LogEntry entry)
    {
        ArgumentNullException.ThrowIfNull(entry);
        memorySink.Log(entry);
        lock (gate)
        {
            pending.Add(entry);
        }
    }

    /// <inheritdoc />
    public void Log(string message)
    {
        Log(new LogEntry(message));
    }

    /// <inheritdoc />
    public IReadOnlyList<LogEntry> GetRecent(int count) => memorySink.GetRecent(count);

    /// <inheritdoc />
    public IReadOnlyList<LogEntry> GetAll() => memorySink.GetAll();

    /// <summary>
    /// Call once per rendered frame. Flushes to file every configured interval.
    /// </summary>
    public void AdvanceFrame()
    {
        frameCounter++;
        if (frameCounter % flushEveryNFrames == 0)
            Flush();
    }

    /// <inheritdoc />
    public void Flush()
    {
        List<LogEntry> toWrite;
        lock (gate)
        {
            if (pending.Count == 0)
                return;

            toWrite = new List<LogEntry>(pending);
            pending.Clear();
        }

        fileSink.AppendRange(toWrite);
    }
}
