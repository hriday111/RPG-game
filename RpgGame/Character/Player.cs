using System.Numerics;
using RpgGame.Core;
using RpgGame.Items;
namespace RpgGame.Character;
/// <summary>
/// Represents the player-controlled character in the game.
/// </summary>
/// <remarks>
/// The Player class manages character attributes, inventory,
/// equipment handling, and currency. Equipment rules are enforced
/// through polymorphic behavior defined in <see cref="IEquippable"/>.
/// </remarks>
public class Player : Character
{
    /// <summary>
    /// Character used to represent the player on the map.
    /// </summary>
    public override char Symbol => '¶';

    #region Attributes

    /// <summary>Gets the player's Strength attribute.</summary>
    public int Strength { get; private set; }

    /// <summary>Gets the player's Dexterity attribute.</summary>
    public int Dexterity { get; private set; }

    /// <summary>Gets the player's current Health value.</summary>
    public int Health { get; private set; }

    /// <summary>Gets the player's Luck attribute.</summary>
    public int Luck { get; private set; }

    /// <summary>Gets the player's Aggression attribute.</summary>
    public int Aggression { get; private set; }

    /// <summary>Gets the player's Wisdom attribute.</summary>
    public int Wisdom { get; private set; }

    #endregion

    #region Equipment

    /// <summary>
    /// Gets the item currently equipped in the left hand.
    /// </summary>
    public IEquippable? LeftHand { get; private set; }

    /// <summary>
    /// Gets the item currently equipped in the right hand.
    /// </summary>
    public IEquippable? RightHand { get; private set; }

    #endregion
    #region Stat Logic

    /// <summary>
    /// Restores the player's health by the specified amount, capped at 100.
    /// </summary>
    /// <param name="amount">The amount of health to restore.</param>
    public void RestoreHealth(int amount)
    {
        Health += amount;
        if (Health > 100)
            Health = 100;
    }

    /// <summary>
    /// Reduces the player's health by the specified amount.
    /// </summary>
    /// <param name="amount">The amount of damage to take.</param>
    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health < 0)
            Health = 0;
    }

    #endregion

    #region Currency

    /// <summary>
    /// Gets the total number of coins collected.
    /// </summary>
    public int Coins { get; private set; }
    public int Gold { get; private set; }
    /// <summary>
    /// Adds the specified amount of coins to the player.
    /// </summary>
    /// <param name="amount">The number of coins to add.</param>
    public void AddCoins(int amount) => Coins += amount;
    public void AddGold(int amount) => Gold += amount;
    #endregion


    #region Equipment Logic (Internal)

    /// <summary>
    /// Equips a one-handed item in the left hand if available.
    /// </summary>
    /// <param name="item">The item to equip.</param>
    /// <returns>True if successfully equipped; otherwise false.</returns>
    internal bool InternalEquipLeft(IEquippable item)
    {
        if (LeftHand != null)
            return false;

        LeftHand = item;
        return true;
    }

    /// <summary>
    /// Equips a one-handed item in the right hand if available.
    /// </summary>
    /// <param name="item">The item to equip.</param>
    /// <returns>True if successfully equipped; otherwise false.</returns>
    internal bool InternalEquipRight(IEquippable item)
    {
        if (RightHand != null)
            return false;

        RightHand = item;
        return true;
    }

    /// <summary>
    /// Equips a two-handed item, occupying both hands.
    /// </summary>
    /// <param name="item">The two-handed item to equip.</param>
    /// <returns>True if successfully equipped.</returns>
    internal bool InternalEquipTwoHand(IEquippable item)
    {
        LeftHand = item;
        RightHand = item;
        return true;
    }

    #endregion

    #region Public Equipment API

    /// <summary>
    /// Attempts to equip an item into the left hand.
    /// </summary>
    /// <param name="item">The equippable item.</param>
    /// <returns>True if the item was successfully equipped; otherwise false.</returns>
    public bool EquipLeft(IEquippable item)
    {
        return item.TryEquipToLeft(this);
    }

    /// <summary>
    /// Attempts to equip an item into the right hand.
    /// </summary>
    /// <param name="item">The equippable item.</param>
    /// <returns>True if the item was successfully equipped; otherwise false.</returns>
    public bool EquipRight(IEquippable item)
    {
        return item.TryEquipToRight(this);
    }

    #endregion

    #region Drop Logic

    /// <summary>
    /// Drops the currently equipped left-hand item onto the level.
    /// </summary>
    /// <param name="level">The current level.</param>
    public void DropLeft(Level level)
    {
        if (LeftHand == null)
            return;

        var item = LeftHand;

        if (LeftHand == RightHand)
        {
            LeftHand = null;
            RightHand = null;
        }
        else
        {
            LeftHand = null;
        }

        item.OnDrop(level, this);
    }

    /// <summary>
    /// Drops the currently equipped right-hand item onto the level.
    /// </summary>
    /// <param name="level">The current level.</param>
    public void DropRight(Level level)
    {
        if (RightHand == null)
            return;

        var item = RightHand;

        if (LeftHand == RightHand)
        {
            LeftHand = null;
            RightHand = null;
        }
        else
        {
            RightHand = null;
        }

        item.OnDrop(level, this);
    }

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="Player"/> class.
    /// </summary>
    /// <param name="startPosition">The initial spawn position of the player.</param>
    public Player(Position startPosition)
        : base(startPosition)
    {
        Strength = 5;
        Dexterity = 5;
        Health = 100;
        Luck = 3;
        Aggression = 4;
        Wisdom = 6;
    }
}
