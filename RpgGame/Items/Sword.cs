namespace RpgGame.Items;

/// <summary>
/// Represents a standard one-handed sword weapon.
/// </summary>
/// <remarks>
/// The <see cref="Sword"/> is a concrete implementation of <see cref="Weapon"/>
/// that uses the <see cref="OneHandOccupation"/> strategy, allowing it to be
/// equipped independently in either the left or right hand.
///
/// This class demonstrates how weapon behavior is configured through
/// composition rather than conditional logic.
/// </remarks>
public class Sword : Weapon
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Sword"/> class.
    /// </summary>
    /// <remarks>
    /// The sword occupies only one hand when equipped.
    /// </remarks>
    public Sword() : base(new OneHandOccupation()) { }

    /// <summary>
    /// Gets the display name of the weapon.
    /// </summary>
    public override string Name => "Sword";
    /// <summary>
    /// Gets the console color used to render the sword on the map.
    /// </summary>
    public override ConsoleColor color => ConsoleColor.Red;
    /// <summary>
    /// Gets the character symbol used to render the sword on the map.
    /// </summary>
    public override char Symbol => '†';

    /// <summary>
    /// Gets the damage value dealt by the sword.
    /// </summary>
    public override int Damage => 10;
}