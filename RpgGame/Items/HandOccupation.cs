using RpgGame.Character;

namespace RpgGame.Items;

/// <summary>
/// Defines the strategy for how an equippable item occupies
/// the player's hands.
/// </summary>
/// <remarks>
/// <para>
/// The <see cref="HandOccupation"/> abstraction encapsulates the logic
/// that determines how an item is equipped. Concrete implementations
/// define whether an item occupies one hand or both hands.
/// </para>
/// <para>
/// This design follows the Strategy Pattern, allowing equipment behavior
/// to vary independently from the <see cref="Weapon"/> class without
/// relying on type checks or enumerations.
/// </para>
/// <para>
/// Implementations must define how equipping behaves when targeting
/// the left or right hand.
/// </para>
/// </remarks>
public abstract class HandOccupation
{
    /// <summary>
    /// Attempts to equip the specified item into the player's left hand.
    /// </summary>
    /// <param name="player">The player attempting to equip the item.</param>
    /// <param name="item">The equippable item.</param>
    /// <returns>
    /// True if the item was successfully equipped; otherwise false.
    /// </returns>
    public abstract bool EquipLeft(Player player, IEquippable item);

    /// <summary>
    /// Attempts to equip the specified item into the player's right hand.
    /// </summary>
    /// <param name="player">The player attempting to equip the item.</param>
    /// <param name="item">The equippable item.</param>
    /// <returns>
    /// True if the item was successfully equipped; otherwise false.
    /// </returns>
    public abstract bool EquipRight(Player player, IEquippable item);
}