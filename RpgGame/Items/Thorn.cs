using RpgGame.Character;
using RpgGame.Core;

namespace RpgGame.Items;

/// <summary>
/// Represents a Thorn on that map, and walking over it causes damange
/// </summary>
public class Thorn : IItem
{
    /// <summary>
    /// Gets the name of the potion.
    /// </summary>
    public string Name => "Thorn";

    /// <summary>
    /// Gets the character symbol representing the potion on the map.
    /// </summary>
    public char Symbol => '^';

    /// <summary>
    /// Gets the color of the potion when rendered.
    /// </summary>
    public ConsoleColor color => ConsoleColor.Red;

    /// <summary>
    /// Returns a description of the potion's effect.
    /// </summary>
    /// <returns>A string describing the potion.</returns>
    public string GetDescription() => "A Thorn that damanges player when stepped on.";

    /// <summary>
    /// Applies damage to the player when they step on the thorn.
    /// </summary>
    /// <param name="player">The player picking up the potion.</param>
    /// <param name="inventory">The player's inventory (not used as the potion is consumed).</param>
    /// <returns>False, as thorns are not removed from the map when stepped on.</returns>
    public bool OnPickup(Player player, Inventory inventory)
    {
        player.TakeDamage(50); // Take 10 damage
        return false; // Thorns are not removed from the map when stepped on.
    }

    /// <summary>
    /// Just here to satisty the IItem interface.
    /// </summary>
    /// <param name="level">The current level.</param>
    /// <param name="player">The player dropping the potion.</param>
    public void OnDrop(Level level, Player player)
    {
        // Thorns are not items that can be dropped, so this method does nothing.
    }
}
