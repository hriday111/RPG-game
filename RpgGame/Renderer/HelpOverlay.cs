using System;
using System.Collections.Generic;
using System.Linq;

namespace RpgGame.Renderer;

/// <summary>
/// Represents a single entry displayed in the help popup.
/// </summary>
public sealed class HelpEntry
{
    public string KeyText { get; }
    public string Description { get; }

    public HelpEntry(string keyText, string description)
    {
        KeyText = keyText;
        Description = description;
    }
}

/// <summary>
/// Responsible for storing and rendering a list of controls/help
/// information as an overlay/popup in the console.
/// </summary>
public class HelpOverlay
{
    private readonly List<HelpEntry> entries = new List<HelpEntry>();

    public void Add(string keyText, string description)
        => entries.Add(new HelpEntry(keyText, description));

    public IReadOnlyList<HelpEntry> Entries => entries;

    /// <summary>
    /// Draws the help popup centered on the console and waits for any
    /// key press before returning (pauses the game loop).
    /// </summary>
    public void Show()
    {
        // calculate dimensions
        int contentWidth = entries.Select(e => e.KeyText.Length + 3 + e.Description.Length).DefaultIfEmpty(0).Max();
        string title = "CONTROLS";
        contentWidth = Math.Max(contentWidth, title.Length);
        int padding = 2;
        int width = contentWidth + padding * 2;
        int height = entries.Count + 4; // title + border + blank

        int startX = Math.Max(0, 0);
        int startY = Math.Max(0, 0);

        // clear entire console before drawing to avoid remnants
        Console.Clear();
        // draw box
        for (int y = 0; y < height; y++)
        {
            Console.SetCursorPosition(startX, startY + y);
            if (y == 0 || y == height - 1)
            {
                Console.Write(new string('*', width));
            }
            else
            {
                Console.Write('*');
                Console.Write(new string(' ', width - 2));
                Console.Write('*');
            }
        }

        // title
        Console.SetCursorPosition(startX + padding, startY + 1);
        Console.Write(title);

        // entries
        for (int i = 0; i < entries.Count; i++)
        {
            var e = entries[i];
            Console.SetCursorPosition(startX + padding, startY + 2 + i);
            Console.Write($"{e.KeyText.PadRight(10)} : {e.Description}");
        }

        // footer
        Console.SetCursorPosition(startX + padding, startY + height - 1);
        Console.Write("Press any key to continue...");

        Console.ReadKey(true);
        Console.Clear();
    }
}
