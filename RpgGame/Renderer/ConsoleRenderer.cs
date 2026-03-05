using RpgGame.Core;
using RpgGame.Character;
using System.ComponentModel.DataAnnotations;

namespace RpgGame.Rendering;

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
/// Rendering prioritizes player → item → terrain.
/// </remarks>
public class ConsoleRenderer
{
    /// <summary>
    /// Renders the current level state and player information to the console.
    /// </summary>
    /// <param name="level">The current level.</param>
    /// <param name="player">The player to render.</param>
    public void Render(Level level, Character.Player player, Inventory inventory)
    {
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
                        Console.Write(item.Symbol);
                    else
                        Console.Write(level.GetTile(x, y).Symbol);
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
    /// Builds the sidebar content displaying character stats,
    /// equipment, currency, and inventory.
    /// </summary>
    /// <returns>A list of formatted sidebar lines.</returns>
    /// <remarks>
    /// There is one very stupid bug that I can't figure out how to fix. Because this rendering logic 
    /// updates only what params change, when a Double sword is equipped, then dropped. The data on the menu
    /// doesn't update properly and shows a weird mix of Sword and Empty. So for now I added a padding of 15 to the right
    /// so the whole line gets cleared and rewritten. This is a band-aid solution. A maybe better solution would be to 
    /// redraw the whole sidebar every frame, and I can change it to an async method to avoid performance issues. But for now I leave this as is and will figure out the 
    /// ideal solution based on the future requirements of the game
    /// </remarks>
    private List<string> GetSidebarContent(Player player, Level level, Inventory inventory)
    {
        var lines = new List<string>
        {
            "=== CHARACTER ===",
            $"STR: {player.Strength} \tDEX: {player.Dexterity}",
            $"HP : {player.Health} \tLCK: {player.Luck}",
            $"AGG: {player.Aggression} \tWIS: {player.Wisdom}",
            "",
            "=== EQUIPMENT ===",
            $"Left : {(player.LeftHand?.Name ?? "Empty").PadRight(15)}\tRight: {(player.RightHand?.Name ?? "Empty").PadRight(15)}",
            "",
            "=== CURRENCY ===",
            $"Coins: {player.Coins} \tGold: {player.Gold}",
            "",
            "=== CURRENT TILE",
            //$"Item Count on tile: {level.TotalItemsAt(player.Pos)}",
            $"Top Item on Tile: {level.GetTopItem(player.Pos)?.Name ?? "No Item here"}",
            "",
            "=== INVENTORY ===",
            //$"Item Description: {inventory.GetTopItem()?.Name ?? "null"}",
        };

        /// <summary>
        /// Adds up to 5 inventory items to the sidebar content.
        /// </summary>
        /// <remarks>
        /// This loop iterates through the player's inventory and adds
        /// the name of each item to the sidebar content, up to a maximum of 5 items.
        /// </remarks>
        for(int i=0; i<Math.Min(5, inventory.GetCount()); i++)
        {
            var item = inventory.GetNItem(i);
            if (item == null) continue;
            {
                lines.Add($"Item {i+1} : {item.Name}");
            }
        }
        return lines;
    }
}