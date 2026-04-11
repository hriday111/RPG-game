using RpgGame.Character;
using RpgGame.Items;

namespace RpgGame.Combat;

/// <summary>
/// Magical strike: magical weapons use full category damage; other weapons deal 1.
/// </summary>
public sealed class MagicalStrikeAttack : ICombatAttack
{
    public static MagicalStrikeAttack Instance { get; } = new();

    private MagicalStrikeAttack() { }

    public void VisitHeavy(IWeapon weapon, Player player, ICombatContribution contribution)
    {
        contribution.AddDamage(1);
        contribution.AddDefense(player.Luck);
    }

    public void VisitLight(IWeapon weapon, Player player, ICombatContribution contribution)
    {
        contribution.AddDamage(1);
        contribution.AddDefense(player.Luck);
    }

    public void VisitMagical(IWeapon weapon, Player player, ICombatContribution contribution)
    {
        contribution.AddDamage(NormalAttack.BaseMagicalDamage(weapon, player));
        contribution.AddDefense(player.Wisdom * 2);
    }

    public void VisitEquippedNonWeapon(IEquippable item, Player player, ICombatContribution contribution)
    {
        contribution.AddDamage(0);
        contribution.AddDefense(player.Luck);
    }

    public void VisitBareFists(Player player, ICombatContribution contribution)
    {
        contribution.AddDamage(1);
        contribution.AddDefense(player.Luck);
    }
}
