using RpgGame.Character;
using RpgGame.Items;

namespace RpgGame.Combat;

/// <summary>
/// Stealth strike: light damage doubled, heavy halved, magical weapon damage 1.
/// </summary>
public sealed class StealthAttack : ICombatAttack
{
    public static StealthAttack Instance { get; } = new();

    private StealthAttack() { }

    public void VisitHeavy(IWeapon weapon, Player player, ICombatContribution contribution)
    {
        contribution.AddDamage(NormalAttack.BaseHeavyDamage(weapon, player) / 2);
        contribution.AddDefense(player.Strength);
    }

    public void VisitLight(IWeapon weapon, Player player, ICombatContribution contribution)
    {
        contribution.AddDamage(NormalAttack.BaseLightDamage(weapon, player) * 2);
        contribution.AddDefense(player.Dexterity);
    }

    public void VisitMagical(IWeapon weapon, Player player, ICombatContribution contribution)
    {
        contribution.AddDamage(1);
        contribution.AddDefense(0);
    }

    public void VisitEquippedNonWeapon(IEquippable item, Player player, ICombatContribution contribution)
    {
        contribution.AddDamage(0);
        contribution.AddDefense(0);
    }

    public void VisitBareFists(Player player, ICombatContribution contribution)
    {
        int baseHeavy = player.Strength + player.Aggression;
        contribution.AddDamage(Math.Max(1, baseHeavy / 2));
        contribution.AddDefense(0);
    }
}
