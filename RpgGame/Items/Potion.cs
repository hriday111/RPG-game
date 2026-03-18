using RpgGame.Character;
using RpgGame.Core;

namespace RpgGame.Items;

/// <summary>
/// Represents a consumable health potion.
/// </summary>
public class Potion : IItem
{
    /// <summary>
    /// Gets the name of the potion.
    /// </summary>
    public string Name => "Potion";

    /// <summary>
    /// Gets the character symbol representing the potion on the map.
    /// </summary>
    public char Symbol => '!';

    /// <summary>
    /// Gets the color of the potion when rendered.
    /// </summary>
    public ConsoleColor color => ConsoleColor.Magenta;

    /// <summary>
    /// Returns a description of the potion's effect.
    /// </summary>
    /// <returns>A string describing the potion.</returns>
    public string GetDescription() => "A magical potion that restores health.";

    /// <summary>
    /// Called when the player picks up the potion. Restores health immediately.
    /// </summary>
    /// <param name="player">The player picking up the potion.</param>
    /// <param name="inventory">The player's inventory (not used as the potion is consumed).</param>
    /// <returns>True, as the potion is consumed immediately.</returns>
    public bool OnPickup(Player player, Inventory inventory)
    {
        player.RestoreHealth(20); // Restore 20 health points
        return true; // Potion is consumed immediately
    }

    /// <summary>
    /// Called when the potion is dropped. Potions are auto-consumed and cannot be dropped.
    /// </summary>
    /// <param name="level">The current level.</param>
    /// <param name="player">The player dropping the potion.</param>
    public void OnDrop(Level level, Player player)
    {
        // Potions are auto-consumed and cannot be dropped.
    }
}
