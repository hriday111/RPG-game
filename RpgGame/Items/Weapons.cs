using RpgGame.Character;
using RpgGame.Core;

namespace RpgGame.Items;

/// <summary>
/// Represents an abstract base class for all equippable weapons.
/// </summary>
/// <remarks>
/// The <see cref="Weapon"/> class implements <see cref="IEquippable"/>
/// and delegates equipment behavior to a <see cref="HandOccupation"/>
/// strategy object. This design allows different weapons to define
/// whether they occupy one or both hands without using type checks
/// or enumerations.
///
/// Concrete weapon types must provide their own name, symbol,
/// and damage value.
/// </remarks>
public abstract class Weapon : IEquippable
{
    /// <summary>
    /// Defines how this weapon occupies the player's hands when equipped.
    /// </summary>
    private readonly HandOccupation occupation;

    /// <summary>
    /// Initializes a new instance of the <see cref="Weapon"/> class
    /// with the specified hand occupation strategy.
    /// </summary>
    /// <param name="occupation">
    /// The strategy that determines how the weapon is equipped
    /// (e.g., one-handed or two-handed).
    /// </param>
    protected Weapon(HandOccupation occupation)
    {
        this.occupation = occupation;
    }

    /// <summary>
    /// Gets the display name of the weapon.
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// Gets the character symbol used to render the weapon on the map.
    /// </summary>
    public abstract char Symbol { get; }

    /// <summary>
    /// Gets the damage value dealt by the weapon.
    /// </summary>
    public abstract int Damage { get; }

    /// <summary>
    /// Attempts to equip the weapon in the player's left hand.
    /// </summary>
    /// <param name="player">The player attempting to equip the weapon.</param>
    /// <returns>
    /// True if the weapon was successfully equipped; otherwise false.
    /// </returns>
    public bool TryEquipToLeft(Player player)
        => occupation.EquipLeft(player, this);

    /// <summary>
    /// Attempts to equip the weapon in the player's right hand.
    /// </summary>
    /// <param name="player">The player attempting to equip the weapon.</param>
    /// <returns>
    /// True if the weapon was successfully equipped; otherwise false.
    /// </returns>
    public bool TryEquipToRight(Player player)
        => occupation.EquipRight(player, this);

    /// <summary>
    /// Returns a short textual description of the weapon.
    /// </summary>
    /// <returns>
    /// A string containing the weapon's name and damage value.
    /// </returns>
    public virtual string GetDescription()
        => $"{Name} (Damage: {Damage})";

    /// <summary>
    /// Handles pickup interaction for the weapon.
    /// </summary>
    /// <param name="player">The player picking up the weapon.</param>
    /// <param name="pickUpLeft">
    /// Indicates whether the pickup action targets the left hand.
    /// If false, the weapon attempts to equip in the right hand.
    /// </param>
    /// <returns>
    /// True if the weapon was successfully equipped; otherwise false.
    /// </returns>
    /*public bool OnPickup(Player player, bool pickUpLeft)
    {
        if (pickUpLeft)
        {
            return TryEquipToLeft(player);
        }
        else
        {
            return TryEquipToRight(player);
        }
    }*/

    public bool OnPickup(Player player, Inventory inventory)
    {
        return  inventory.AddToInventory(this);
    }

    
    /// <summary>
    /// Drops the weapon onto the current level at the player's position.
    /// </summary>
    /// <param name="level">The current level.</param>
    /// <param name="player">The player dropping the weapon.</param>
    public void OnDrop(Level level, Player player)
    {
        level.AddItem(player.Pos, this);
    }
}