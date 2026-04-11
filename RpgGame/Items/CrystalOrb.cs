using RpgGame.Combat;

namespace RpgGame.Items;

/// <summary>
/// One-handed focus weapon: magical category (damage scales with wisdom in combat).
/// </summary>
public sealed class CrystalOrb : Weapon
{
    /// <inheritdoc />
    protected override IWeaponCategory CombatCategory => MagicalWeaponCategory.Instance;

    /// <summary>
    /// Initializes a new instance of the <see cref="CrystalOrb"/> class.
    /// </summary>
    public CrystalOrb() : base(new OneHandOccupation()) { }

    /// <inheritdoc />
    public override string Name => "Crystal Orb";

    /// <inheritdoc />
    public override ConsoleColor color => ConsoleColor.Cyan;

    /// <inheritdoc />
    public override char Symbol => '◉';

    /// <inheritdoc />
    public override int Damage => 8;
}
