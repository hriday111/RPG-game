using RpgGame.Character;

namespace RpgGame.Items;

/// <summary>
/// Represents a hand occupation strategy for one-handed equipment.
/// </summary>
/// <remarks>
/// The <see cref="OneHandOccupation"/> class defines the behavior
/// for items that occupy only a single hand when equipped.
///
/// This strategy allows a weapon to be equipped independently
/// in either the left or right hand, provided that the selected
/// hand is currently unoccupied.
///
/// This class is part of the equipment strategy system and works
/// in conjunction with <see cref="HandOccupation"/> to remove the
/// need for conditional logic or type checking when equipping items.
/// </remarks>
public sealed class OneHandOccupation : HandOccupation
{
    /// <summary>
    /// Attempts to equip the item in the player's left hand.
    /// </summary>
    /// <param name="player">The player attempting to equip the item.</param>
    /// <param name="item">The item being equipped.</param>
    /// <returns>
    /// True if the left hand was free and the item was successfully equipped;
    /// otherwise false.
    /// </returns>
    public override bool EquipLeft(Player player, IEquippable item)
    {
        return player.InternalEquipLeft(item);
    }

    /// <summary>
    /// Attempts to equip the item in the player's right hand.
    /// </summary>
    /// <param name="player">The player attempting to equip the item.</param>
    /// <param name="item">The item being equipped.</param>
    /// <returns>
    /// True if the right hand was free and the item was successfully equipped;
    /// otherwise false.
    /// </returns>
    public override bool EquipRight(Player player, IEquippable item)
    {
        return player.InternalEquipRight(item);
    }
}