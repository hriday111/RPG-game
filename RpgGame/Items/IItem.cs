using RpgGame.Character;
using RpgGame.Core;

namespace RpgGame.Items;

/// <summary>
/// Defines the contract for all items that can exist within the game world.
/// </summary>
/// <remarks>
/// Implementations of <see cref="IItem"/> represent objects that can:
/// <list type="bullet">
/// <item><description>Be displayed on the map using a symbol.</description></item>
/// <item><description>Provide a textual description.</description></item>
/// <item><description>React to pickup interactions.</description></item>
/// <item><description>Be dropped onto the level.</description></item>
/// </list>
/// 
/// The pickup behavior may vary depending on the item type. For example,
/// equippable items attempt to occupy a specific hand, while consumable
/// items (such as coins) apply their effect immediately.
/// </remarks>
public interface IItem
{
    /// <summary>
    /// Gets the display name of the item.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the character symbol used to render the item on the map.
    /// </summary>
    char Symbol { get; }

    /// <summary>
    /// Returns a short textual description of the item.
    /// </summary>
    /// <returns>A human-readable description of the item.</returns>
    string GetDescription();

    /// <summary>
    /// Handles the pickup interaction for the item.
    /// </summary>
    /// <param name="player">The player interacting with the item.</param>
    /// <param name="pickUpLeft">
    /// Indicates whether the pickup action targets the left hand.
    /// This parameter is primarily relevant for equippable items.
    /// </param>
    /// <returns>
    /// True if the pickup action was successful; otherwise false.
    /// </returns>
    //bool OnPickup(Player player, bool pickUpLeft);
    bool OnPickup(Player player, Inventory inventory);

    /// <summary>
    /// Handles the drop interaction for the item.
    /// </summary>
    /// <param name="level">The current level.</param>
    /// <param name="player">The player dropping the item.</param>
    void OnDrop(Level level, Player player);
}