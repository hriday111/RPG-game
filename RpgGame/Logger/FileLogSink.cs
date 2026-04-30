namespace RpgGame.Logger;

/// <summary>
/// Persists log entries to a dedicated text file for the current game session.
/// </summary>
public sealed class FileLogSink : ILogSink
{
    private readonly object gate = new();
    private readonly string filePath;

    /// <summary>
    /// Gets the path of the log file created for this session.
    /// </summary>
    public string FilePath => filePath;

    /// <summary>
    /// Creates a file sink and allocates a unique session file.
    /// </summary>
    /// <param name="directory">Directory where logs are stored.</param>
    /// <param name="playerName">Player name used in the file name.</param>
    /// <param name="sessionStart">Session start time used in file naming.</param>
    public FileLogSink(string directory, string playerName, DateTime sessionStart)
    {
        if (string.IsNullOrWhiteSpace(directory))
            throw new ArgumentException("Log directory must not be empty.", nameof(directory));
        if (string.IsNullOrWhiteSpace(playerName))
            throw new ArgumentException("Player name must not be empty.", nameof(playerName));

        Directory.CreateDirectory(directory);
        string safeName = MakeSafeFileName(playerName);
        string baseName = $"{safeName}_{sessionStart:yyyyMMdd_HHmmss}";
        filePath = CreateUniquePath(directory, baseName, ".log");
    }

    /// <inheritdoc />
    public void Log(LogEntry entry)
    {
        ArgumentNullException.ThrowIfNull(entry);
        AppendLines(new[] { entry.ToString() });
    }

    /// <inheritdoc />
    public void Log(string message)
    {
        Log(new LogEntry(message));
    }

    /// <summary>
    /// Appends multiple entries in one write operation.
    /// </summary>
    /// <param name="entries">Entries to append.</param>
    public void AppendRange(IEnumerable<LogEntry> entries)
    {
        ArgumentNullException.ThrowIfNull(entries);
        AppendLines(entries.Select(e => e.ToString()));
    }

    /// <inheritdoc />
    public IReadOnlyList<LogEntry> GetRecent(int count) => Array.Empty<LogEntry>();

    /// <inheritdoc />
    public IReadOnlyList<LogEntry> GetAll() => Array.Empty<LogEntry>();

    /// <inheritdoc />
    public void Flush()
    {
        // Writes are immediate; no buffered state in this sink.
    }

    private void AppendLines(IEnumerable<string> lines)
    {
        lock (gate)
        {
            File.AppendAllLines(filePath, lines);
        }
    }

    private static string MakeSafeFileName(string rawName)
    {
        var invalid = Path.GetInvalidFileNameChars();
        var filtered = new string(rawName.Where(c => !invalid.Contains(c)).ToArray());
        return string.IsNullOrWhiteSpace(filtered) ? "Player" : filtered;
    }

    private static string CreateUniquePath(string directory, string baseName, string extension)
    {
        for (int i = 0; i < int.MaxValue; i++)
        {
            string suffix = i == 0 ? string.Empty : $"_{i}";
            string candidate = Path.Combine(directory, $"{baseName}{suffix}{extension}");
            try
            {
                using var _ = new FileStream(candidate, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
                return candidate;
            }
            catch (IOException)
            {
                // Candidate already exists, try next suffix.
            }
        }

        throw new IOException("Could not create a unique log file name.");
    }
}
