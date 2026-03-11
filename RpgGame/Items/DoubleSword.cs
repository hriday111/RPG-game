namespace RpgGame.Items;

/// <summary>
/// Represents a powerful two-handed sword weapon.
/// </summary>
/// <remarks>
/// The <see cref="DoubleSword"/> occupies both hands when equipped,
/// as defined by the <see cref="TwoHandOccupation"/> strategy passed
/// to the base <see cref="Weapon"/> class.
///
/// This class demonstrates the use of composition to determine
/// equipment behavior without relying on type checks.
/// </remarks>
public class DoubleSword : Weapon
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DoubleSword"/> class.
    /// </summary>
    /// <remarks>
    /// The weapon uses a <see cref="TwoHandOccupation"/> strategy,
    /// meaning it requires both hands to be free before equipping.
    /// </remarks>
    public DoubleSword() : base(new TwoHandOccupation()) { }

    /// <summary>
    /// Gets the display name of the weapon.
    /// </summary>
    public override string Name => "Double Sword";
    /// <summary>
    /// Gets the console color used to render the Double Sword on the map.
    /// </summary>
    public override ConsoleColor color => ConsoleColor.DarkRed;
    /// <summary>
    /// Gets the character symbol used to render the weapon on the map.
    /// </summary>
    public override char Symbol => '⚔';

    /// <summary>
    /// Gets the damage value dealt by this weapon.
    /// </summary>
    public override int Damage => 25;
}