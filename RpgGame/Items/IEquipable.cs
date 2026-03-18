using RpgGame.Character;

namespace RpgGame.Items;

/// <summary>
/// Represents an item that can be equipped by the player.
/// </summary>
/// <remarks>
/// <para>
/// The <see cref="IEquippable"/> interface extends <see cref="IItem"/>
/// and defines behavior specific to items that can occupy the player's hands.
/// </para>
/// <para>
/// Implementations determine whether equipping succeeds based on
/// current equipment state and occupation rules. This abstraction
/// enables polymorphic equipment handling without relying on
/// type checks or enumerations.
/// </para>
/// </remarks>
public interface IEquippable : IItem
{
    /// <summary>
    /// Attempts to equip the item in the player's left hand.
    /// </summary>
    /// <param name="player">The player attempting to equip the item.</param>
    /// <returns>
    /// True if the item was successfully equipped in the left hand;
    /// otherwise false.
    /// </returns>
    bool TryEquipToLeft(Player player);

    /// <summary>
    /// Attempts to equip the item in the player's right hand.
    /// </summary>
    /// <param name="player">The player attempting to equip the item.</param>
    /// <returns>
    /// True if the item was successfully equipped in the right hand;
    /// otherwise false.
    /// </returns>
    bool TryEquipToRight(Player player);
}
