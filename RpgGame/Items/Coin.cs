using RpgGame.Character;
using RpgGame.Core;

namespace RpgGame.Items;

/// <summary>
/// Represents a coin item that increases the player's currency when picked up.
/// </summary>
/// <remarks>
/// Coins are automatically consumed upon pickup and are not stored
/// in the player's inventory. The <paramref name="pickUpLeft"/> parameter
/// is accepted to conform to the item interaction signature, although
/// it has no effect on coin behavior.
/// </remarks>
public class Coin : IItem
{
    /// <summary>
    /// Gets the display name of the coin.
    /// </summary>
    public string Name => "Coin";

    /// <summary>
    /// Gets the character symbol used to render the coin on the map.
    /// </summary>
    public char Symbol => 'c';

    /// <summary>
    /// Gets the console color used to render the Coin on the map.
    /// </summary>
    public ConsoleColor color => ConsoleColor.Yellow;
    /// <summary>
    /// Returns a short description of the coin.
    /// </summary>
    /// <returns>A textual description of the item.</returns>
    public string GetDescription() => "A small coin.";

    /// <summary>
    /// Handles the pickup interaction for the coin.
    /// </summary>
    /// <param name="player">The player picking up the coin.</param>
    /// <param name="pickUpLeft">
    /// Indicates whether the pickup interaction was initiated for the left hand.
    /// This parameter is ignored for coins.
    /// </param>
    /// <returns>
    /// Always returns true, as coins are immediately consumed and applied to the player.
    /// </returns>
    public bool OnPickup(Player player, Inventory inventory)
    {
        player.AddCoins(1);
        return true;
    }

    /// <summary>
    /// Handles drop behavior for the coin.
    /// </summary>
    /// <param name="level">The current level.</param>
    /// <param name="player">The player attempting to drop the coin.</param>
    /// <remarks>
    /// Coins cannot be dropped because they are consumed immediately upon pickup.
    /// </remarks>
    public void OnDrop(Level level, Player player)
    {
        // Coins are auto-consumed and cannot be dropped.
    }
}
