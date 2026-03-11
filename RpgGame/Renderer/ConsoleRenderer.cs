using RpgGame.Core;
using RpgGame.Character;
using System.ComponentModel.DataAnnotations;

namespace RpgGame.Renderer;

/// <summary>
/// Handles rendering the game world and sidebar to the console.
/// </summary>
/// <remarks>
/// The <see cref="ConsoleRenderer"/> is responsible for displaying:
/// - The tile grid
/// - The player position
/// - Items on the ground
/// - The sidebar with character and inventory information
/// 
/// </remarks>
public class ConsoleRenderer
{
    private readonly HelpOverlay help = new HelpOverlay();
    private bool showHelp;

    /// <summary>
    /// Registers a control/help entry that will later be rendered when F1 is
    /// pressed.  The key text should already include any modifier information
    /// (e.g. "Shift+Q").
    /// </summary>
    public void RegisterHelpEntry(string keyText, string description)
    {
        help.Add(keyText, description);
    }

    /// <summary>
    /// External callers can toggle the popup on or off (usually in the game
    /// loop when F1 is detected).
    /// </summary>
    public void ToggleHelpDisplay()
    {
        showHelp = !showHelp;
    }

    /// <summary>
    /// Renders the current level state and player information to the console.
    /// If the help flag is set, a help overlay will be shown instead of the
    /// normal game view.
    /// </summary>
    /// <param name="level">The current level.</param>
    /// <param name="player">The player to render.</param>
    public void Render(Level level, Character.Player player, Inventory inventory)
    {
        if (showHelp)
        {
            help.Show();
            showHelp = false; // hide and then fall through to redraw the map immediately
        }

        Console.SetCursorPosition(0, 0);

        for (int y = 0; y < level.Height; y++)
        {
            for (int x = 0; x < level.Width; x++)
            {
                var currentPos = new Position(x, y);

                if (player.Pos == currentPos)
                {
                    Console.Write(player.Symbol);
                }
                else
                {
                    var item = level.GetTopItem(currentPos);

                    if (item != null)
                    {
                        Console.ForegroundColor = item.color;
                        Console.Write(item.Symbol);
                    }
                    else
                    {
                        Console.ForegroundColor = level.GetTile(x,y).color;
                        Console.Write(level.GetTile(x, y).Symbol);
                    }
                }
            }

            DrawSidebarLine(player, level, inventory, y);
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Draws a single line of the sidebar aligned with the map row.
    /// </summary>
    private void DrawSidebarLine(Player player, Level level, Inventory inventory, int line)
    {
        Console.Write("  "); // spacing

        var sidebarContent = GetSidebarContent(player, level, inventory);

        if (line < sidebarContent.Count)
            Console.Write(sidebarContent[line].PadRight(Config.SidebarWidth));
        else
            Console.Write(new string(' ', Config.SidebarWidth));
    }

    /// <summary>
    /// Generates the content for the sidebar based on the player's current state,
    /// inventory, and other relevant information.  The content is returned as a
    /// list of strings, where each string represents a line in the sidebar.  The caller is responsible for aligning these lines with the map rendering.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="level"></param>
    /// <param name="inventory"></param>
    /// <remarks>
    /// There is a slight visual glitch that the last line closing the menu box is not rendered as it is more than the Console Lenght defined in the <cref="Config"/> file.
    /// <returns>
    /// Returns a list of strings representing the lines to be displayed in the sidebar.  The content includes: 
    /// - Character stats (HP, Luck, Strength, etc.)
    /// - Equipped items (left and right hand)
    /// - Currency (coins, gold)
    /// - Inventory slots (with indicators for selected item)
    /// </returns>
    private List<string> GetSidebarContent(Player player, Level level, Inventory inventory)
    {
        int innerWidth = Config.SidebarWidth - 2; 
        
        string PadLine(string content)
        {
            if (content.Length > innerWidth) content = content.Substring(0, innerWidth);
            return "│" + content.PadRight(innerWidth) + "│";
        }

        var lines = new List<string>
        {   
            "┌" + new string('─', innerWidth) + "┐",
            PadLine("      RPG GAME"),
            "├" + new string('─', innerWidth) + "┤",
            PadLine(" [F1] Help"),
            "├" + new string('─', innerWidth) + "┤",
            PadLine(" CHARACTER"),
            PadLine($" HP : {player.Health,-3} LCK: {player.Luck,-3}"),
            PadLine($" STR: {player.Strength,-3} DEX: {player.Dexterity,-3}"),
            PadLine($" AGG: {player.Aggression,-3} WIS: {player.Wisdom,-3}"),
            "├" + new string('─', innerWidth) + "┤",
            PadLine(" EQUIPMENT"),
            PadLine($" L: {(player.LeftHand?.Symbol.ToString() ?? "-"),-1} {(player.LeftHand?.Name ?? "Empty")}"),
            PadLine($" R: {(player.RightHand?.Symbol.ToString() ?? "-"),-1} {(player.RightHand?.Name ?? "Empty")}"),
            "├" + new string('─', innerWidth) + "┤",
            PadLine(" CURRENCY"),
            PadLine($" Coins: {player.Coins,-5} Gold: {player.Gold,-5}"),
            "├" + new string('─', innerWidth) + "┤",
            PadLine(" INVENTORY"),
        };

        // First row (1-5)
        string row1 = " ";
        for (int i = 0; i < 5; i++)
        {
            var item = inventory.GetNItem(i);
            char sym = item?.Symbol ?? ' ';
            string bOpen = (inventory.SelectedIndex == i) ? ">" : "[";
            string bClose = (inventory.SelectedIndex == i) ? "<" : "]";
            row1 += $"{i + 1}:{bOpen}{sym}{bClose} ";
        }
        lines.Add(PadLine(row1));

        // Second row (6-0)
        string row2 = " ";
        for (int i = 5; i < 10; i++)
        {
            var item = inventory.GetNItem(i);
            char sym = item?.Symbol ?? ' ';
            string bOpen = (inventory.SelectedIndex == i) ? ">" : "[";
            string bClose = (inventory.SelectedIndex == i) ? "<" : "]";
            int num = (i + 1) % 10;
            row2 += $"{num}:{bOpen}{sym}{bClose} ";
        }
        lines.Add(PadLine(row2));

        lines.Add("└" + new string('─', innerWidth) + "┘");

        var selected = inventory.GetSelectedItem();
        if (selected != null)
        {
            lines.Add($" Selected: {selected.Name}");
            string desc = selected.GetDescription();
            if (desc.Length > Config.SidebarWidth - 2) desc = desc.Substring(0, Config.SidebarWidth - 5) + "...";
            lines.Add($" {desc}");
        }
        else
        {
            lines.Add(" Slot Empty");
            lines.Add("");
        }

        return lines;
    }
}