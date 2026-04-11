using RpgGame.Character;
using RpgGame.Items;

namespace RpgGame.Combat;

/// <summary>
/// Visitor for combat resolution: each concrete attack implements how damage and defense
/// combine with heavy, light, and magical weapon categories (double dispatch via
/// <see cref="IWeaponCategory.DispatchCombat"/>).
/// </summary>
public interface ICombatAttack
{
    void VisitHeavy(IWeapon weapon, Player player, ICombatContribution contribution);

    void VisitLight(IWeapon weapon, Player player, ICombatContribution contribution);

    void VisitMagical(IWeapon weapon, Player player, ICombatContribution contribution);

    /// <summary>
    /// Held <see cref="IEquippable"/> that is not a weapon: no damage; defense from attack rules.
    /// </summary>
    void VisitEquippedNonWeapon(IEquippable item, Player player, ICombatContribution contribution);

    /// <summary>
    /// No weapon in either hand: unarmed strike (damage) and minimal defense fallback.
    /// </summary>
    void VisitBareFists(Player player, ICombatContribution contribution);
}
