using RpgGame.Logger;

namespace RpgGame.Renderer;

/// <summary>
/// Renders a journal popup using entries from the in-memory sink (via GameLog).
/// </summary>
public sealed class JournalOverlay
{
    /// <summary>
    /// Draws journal entries and waits for any key to return.
    /// </summary>
    public void Show()
    {
        IReadOnlyList<LogEntry> all = GameLog.All();

        int width = Math.Max(40, Console.WindowWidth - 2);
        int maxRows = Math.Max(6, Console.WindowHeight - 5);
        int start = Math.Max(0, all.Count - maxRows);
        string title = "JOURNAL (latest entries)";

        Console.Clear();
        Console.WriteLine(new string('*', width));
        Console.WriteLine($"* {title.PadRight(width - 4)} *");
        Console.WriteLine(new string('*', width));

        if (all.Count == 0)
        {
            Console.WriteLine("* (no log entries yet)".PadRight(width - 1) + "*");
        }
        else
        {
            for (int i = start; i < all.Count; i++)
            {
                string line = all[i].ToString();
                if (line.Length > width - 4)
                    line = line[..(width - 7)] + "...";

                Console.WriteLine($"* {line.PadRight(width - 4)} *");
            }
        }

        Console.WriteLine(new string('*', width));
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey(true);
        Console.Clear();
    }
}
