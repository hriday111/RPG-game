using RpgGame.Character;
using RpgGame.Combat;
using RpgGame.Core;

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

        List<string> sidebarContent = GetSidebarContent(player, level, inventory);

        for (int y = 0; y < level.Height; y++)
        {
            for (int x = 0; x < level.Width; x++)
            {
                var currentPos = new Position(x, y);

                if (player.Pos == currentPos)
                {
                    Console.Write(player.Symbol);
                }
                else if (level.TryGetGolemAt(currentPos, out Golem? golemHere))
                {
                    Console.ResetColor();
                    Console.Write(golemHere.Symbol);
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
                        Console.ForegroundColor = level.GetTile(x, y).color;
                        Console.Write(level.GetTile(x, y).Symbol);
                    }
                }
            }

            DrawSidebarLine(sidebarContent, y);
            Console.WriteLine();
        }

        // Sidebar is taller than the map: continue under the map, same column as the panel.
        for (int i = level.Height; i < sidebarContent.Count; i++)
        {
            Console.Write(new string(' ', level.Width));
            Console.Write("  ");
            Console.WriteLine(sidebarContent[i].PadRight(Config.SidebarWidth));
        }

        if (player.Health <= 0)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("  GAME OVER — You were defeated. Press any key to exit.");
            Console.ResetColor();
        }
    }

    /// <summary>
    /// Draws one sidebar row beside the map (same index as the map row).
    /// </summary>
    private static void DrawSidebarLine(IReadOnlyList<string> sidebarContent, int line)
    {
        Console.Write("  ");
        if (line < sidebarContent.Count)
            Console.Write(sidebarContent[line].PadRight(Config.SidebarWidth));
        else
            Console.Write(new string(' ', Config.SidebarWidth));
    }

    private static string DescribeSelectedAttack(Player player)
    {
        if (ReferenceEquals(player.SelectedCombatAttack, StealthAttack.Instance))
            return "Stealth (F3)";
        if (ReferenceEquals(player.SelectedCombatAttack, MagicalStrikeAttack.Instance))
            return "Magical (F4)";
        return "Normal (F2)";
    }

    /// <summary>
    /// Builds sidebar lines (stats, equipment, combat, inventory). Rows past the map height are printed below the map, aligned with the panel.
    /// </summary>
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
            PadLine(player.Health <= 0 ? " *** YOU DIED - GAME OVER ***" : $" HP : {player.Health,-3} LCK: {player.Luck,-3}"),
            PadLine($" STR: {player.Strength,-3} DEX: {player.Dexterity,-3}"),
            PadLine($" AGG: {player.Aggression,-3} WIS: {player.Wisdom,-3}"),
            PadLine(player.Health <= 0 ? " Any key quits to desktop." : " "),
            "├" + new string('─', innerWidth) + "┤",
            PadLine(" COMBAT"),
            PadLine($" ATK MODE: {DescribeSelectedAttack(player)}"),
            PadLine(string.IsNullOrEmpty(level.LastCombatMessage) ? " —" : $" {level.LastCombatMessage}"),
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
