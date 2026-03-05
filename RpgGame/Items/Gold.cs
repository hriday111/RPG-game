using RpgGame.Character;
using RpgGame.Core;

namespace RpgGame.Items;

/// <summary>
/// Represents a gold item that increases the player's gold currency when picked up.
/// </summary>
/// <remarks>
/// Gold is automatically consumed upon pickup and is not stored
/// in the player's inventory. The <paramref name="inventory"/> parameter
/// is accepted to conform to the item interaction signature, although
/// it has no effect on gold behavior.
/// </remarks>
public class Gold : IItem
{
    /// <summary>
    /// Gets the display name of the coin.
    /// </summary>
    public string Name => "Gold";

    /// <summary>
    /// Gets the character symbol used to render the coin on the map.
    /// </summary>
    public char Symbol => '☉';

    /// <summary>
    /// Returns a short description of the coin.
    /// </summary>
    /// <returns>A textual description of the item.</returns>
    public string GetDescription() => "A gold piece.";

    /// <summary>
    /// Handles the pickup interaction for the gold item.
    /// </summary>
    /// <param name="player">The player picking up the gold.</param>
    /// <param name="inventory">
    /// The player's inventory object.
    /// This parameter is ignored for gold items.
    /// </param>
    /// <returns>
    /// Always returns true, as gold is immediately consumed and applied to the player.
    /// </returns>
    public bool OnPickup(Player player, Inventory inventory)
    {
        player.AddGold(1);
        return true;
    }

    /// <summary>
    /// Handles drop behavior for the gold item.
    /// </summary>
    /// <param name="level">The current level.</param>
    /// <param name="player">The player attempting to drop the gold.</param>
    /// <remarks>
    /// Gold cannot be dropped because it is consumed immediately upon pickup.
    /// </remarks>
    public void OnDrop(Level level, Player player)
    {
        // Gold is auto-consumed and cannot be dropped.
    }
}