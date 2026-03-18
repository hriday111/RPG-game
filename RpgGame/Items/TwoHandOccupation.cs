using RpgGame.Character;

namespace RpgGame.Items;

/// <summary>
/// Represents a hand occupation strategy for two-handed equipment.
/// </summary>
/// <remarks>
/// The <see cref="TwoHandOccupation"/> class defines the behavior
/// required to equip an item that occupies both the left and right hands.
///
/// This strategy ensures that both hands must be free before equipping
/// the item. If either hand is already occupied, the equip operation fails.
///
/// This class is part of the equipment strategy system and helps
/// eliminate the need for type checks or enumeration-based logic
/// when determining how items are equipped.
/// </remarks>
public sealed class TwoHandOccupation : HandOccupation
{
    /// <summary>
    /// Attempts to equip the item in the left hand.
    /// For two-handed items, this delegates to a shared method
    /// that occupies both hands.
    /// </summary>
    /// <param name="player">The player attempting to equip the item.</param>
    /// <param name="item">The item being equipped.</param>
    /// <returns>
    /// True if both hands were free and the item was successfully equipped;
    /// otherwise false.
    /// </returns>
    public override bool EquipLeft(Player player, IEquippable item)
    {
        return EquipBoth(player, item);
    }

    /// <summary>
    /// Attempts to equip the item in the right hand.
    /// For two-handed items, this delegates to a shared method
    /// that occupies both hands.
    /// </summary>
    /// <param name="player">The player attempting to equip the item.</param>
    /// <param name="item">The item being equipped.</param>
    /// <returns>
    /// True if both hands were free and the item was successfully equipped;
    /// otherwise false.
    /// </returns>
    public override bool EquipRight(Player player, IEquippable item)
    {
        return EquipBoth(player, item);
    }

    /// <summary>
    /// Equips the item in both hands if they are currently unoccupied.
    /// </summary>
    /// <param name="player">The player attempting to equip the item.</param>
    /// <param name="item">The two-handed item to equip.</param>
    /// <returns>
    /// True if the item was successfully equipped in both hands;
    /// otherwise false.
    /// </returns>
    private bool EquipBoth(Player player, IEquippable item)
    {
        if (player.LeftHand != null || player.RightHand != null)
            return false;

        return player.InternalEquipTwoHand(item);
    }
}
