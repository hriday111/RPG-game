using RpgGame.Character;
using RpgGame.Items;

namespace RpgGame.Combat;

public sealed class HeavyWeaponCategory : IWeaponCategory
{
    public static HeavyWeaponCategory Instance { get; } = new();

    private HeavyWeaponCategory() { }

    public void DispatchCombat(ICombatAttack attack, IWeapon weapon, Player player, ICombatContribution contribution)
        => attack.VisitHeavy(weapon, player, contribution);
}

public sealed class LightWeaponCategory : IWeaponCategory
{
    public static LightWeaponCategory Instance { get; } = new();

    private LightWeaponCategory() { }

    public void DispatchCombat(ICombatAttack attack, IWeapon weapon, Player player, ICombatContribution contribution)
        => attack.VisitLight(weapon, player, contribution);
}

public sealed class MagicalWeaponCategory : IWeaponCategory
{
    public static MagicalWeaponCategory Instance { get; } = new();

    private MagicalWeaponCategory() { }

    public void DispatchCombat(ICombatAttack attack, IWeapon weapon, Player player, ICombatContribution contribution)
        => attack.VisitMagical(weapon, player, contribution);
}
