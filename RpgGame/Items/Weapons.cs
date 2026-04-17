using RpgGame.Character;
using RpgGame.Combat;
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
public abstract class Weapon : IWeapon
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
    /// Gets the console color used to render the weapon on the map.
    /// </summary>
    public abstract ConsoleColor color { get; }
    /// <summary>
    /// Gets the character symbol used to render the weapon on the map.
    /// </summary>
    public abstract char Symbol { get; }

    /// <summary>
    /// Gets the damage value dealt by the weapon.
    /// </summary>
    public abstract int Damage { get; }

    /// <inheritdoc />
    public virtual int Defense => 0;

    /// <summary>
    /// Heavy, light, or magical classification for combat visitors.
    /// </summary>
    protected abstract IWeaponCategory CombatCategory { get; }

    /// <inheritdoc />
    public void AcceptCombatStrike(ICombatAttack attack, Player player, ICombatContribution contribution)
        => DispatchCombatUsingSurface(attack, this, player, contribution);

    /// <inheritdoc />
    public virtual void ContributeCombat(ICombatAttack attack, Player player, ICombatContribution contribution)
        => AcceptCombatStrike(attack, player, contribution);

    /// <summary>
    /// Applies <paramref name="attack"/> using this weapon's category but <paramref name="damageSurface"/>'s
    /// <see cref="IWeapon.Damage"/> (so decorators retain category and stacked modifiers).
    /// </summary>
    internal void DispatchCombatUsingSurface(
        ICombatAttack attack,
        IWeapon damageSurface,
        Player player,
        ICombatContribution contribution)
        => CombatCategory.DispatchCombat(attack, damageSurface, player, contribution);

    /// <summary>
    /// Attempts to equip the weapon in the player's left hand.
    /// </summary>
    /// <param name="player">The player attempting to equip the weapon.</param>
    /// <returns>
    /// True if the weapon was successfully equipped; otherwise false.
    /// </returns>
    public bool TryEquipToLeft(Player player)
        => ApplyEquipLeft(player, this);

    /// <summary>
    /// Attempts to equip the weapon in the player's right hand.
    /// </summary>
    /// <param name="player">The player attempting to equip the weapon.</param>
    /// <returns>
    /// True if the weapon was successfully equipped; otherwise false.
    /// </returns>
    public bool TryEquipToRight(Player player)
        => ApplyEquipRight(player, this);

    /// <summary>
    /// Equips using this weapon's hand-occupation rules, but stores
    /// <paramref name="equippedSurface"/> on the player (the outer decorator stack).
    /// </summary>
    internal bool ApplyEquipLeft(Player player, IEquippable equippedSurface)
        => occupation.EquipLeft(player, equippedSurface);

    internal bool ApplyEquipRight(Player player, IEquippable equippedSurface)
        => occupation.EquipRight(player, equippedSurface);

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
        return inventory.AddToInventory(this);
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
