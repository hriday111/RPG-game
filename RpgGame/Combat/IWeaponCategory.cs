using RpgGame.Character;
using RpgGame.Items;

namespace RpgGame.Combat;

/// <summary>
/// Supplies max orthogonal walk steps at which weapon-handling noise remains audible per category.
/// </summary>
public interface INoiseHearingRangeVisitor
{
    int HeavyWeaponPickupHearingSteps();

    int MagicalWeaponPickupHearingSteps();

    int LightWeaponPickupHearingSteps();
}

/// <summary>
/// Default hearing ranges: heavy long, magic moderate, light minimal.
/// </summary>
public static class WeaponPickupNoiseHearingRanges
{
    public static INoiseHearingRangeVisitor Default { get; } = new DefaultVisitor();

    private sealed class DefaultVisitor : INoiseHearingRangeVisitor
    {
        public int HeavyWeaponPickupHearingSteps() => 8;

        public int MagicalWeaponPickupHearingSteps() => 4;

        public int LightWeaponPickupHearingSteps() => 1;
    }
    private sealed class TestVisitor : INoiseHearingRangeVisitor
    {
        public int HeavyWeaponPickupHearingSteps() => 80;

        public int MagicalWeaponPickupHearingSteps() => 84;

        public int LightWeaponPickupHearingSteps() => 81;
    }
}

/// <summary>
/// Weapon classification for combat: dispatches to the active <see cref="ICombatAttack"/>
/// without type tags (each concrete weapon composes one category instance).
/// </summary>
public interface IWeaponCategory
{
    void DispatchCombat(ICombatAttack attack, IWeapon weapon, Player player, ICombatContribution contribution);

    /// <summary>
    /// Double dispatch for pickup/equip noise reach along walkable paths.
    /// </summary>
    int DispatchPickupNoiseHearingRange(INoiseHearingRangeVisitor visitor);
}
