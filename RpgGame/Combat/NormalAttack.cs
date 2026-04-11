using RpgGame.Character;
using RpgGame.Items;

namespace RpgGame.Combat;

/// <summary>
/// Standard strike: heavy/light use full category damage; magical weapons deal 1.
/// </summary>
public sealed class NormalAttack : ICombatAttack
{
    public static NormalAttack Instance { get; } = new();

    private NormalAttack() { }

    public void VisitHeavy(IWeapon weapon, Player player, ICombatContribution contribution)
    {
        contribution.AddDamage(BaseHeavyDamage(weapon, player));
        contribution.AddDefense(player.Strength + player.Luck);
    }

    public void VisitLight(IWeapon weapon, Player player, ICombatContribution contribution)
    {
        contribution.AddDamage(BaseLightDamage(weapon, player));
        contribution.AddDefense(player.Dexterity + player.Luck);
    }

    public void VisitMagical(IWeapon weapon, Player player, ICombatContribution contribution)
    {
        contribution.AddDamage(1);
        contribution.AddDefense(player.Dexterity + player.Luck);
    }

    public void VisitEquippedNonWeapon(IEquippable item, Player player, ICombatContribution contribution)
    {
        contribution.AddDamage(0);
        contribution.AddDefense(player.Dexterity);
    }

    public void VisitBareFists(Player player, ICombatContribution contribution)
    {
        contribution.AddDamage(Math.Max(1, player.Strength + player.Aggression));
        contribution.AddDefense(player.Dexterity);
    }

    internal static int BaseHeavyDamage(IWeapon weapon, Player player)
        => weapon.Damage + player.Strength + player.Aggression;

    internal static int BaseLightDamage(IWeapon weapon, Player player)
        => weapon.Damage + player.Dexterity + player.Luck;

    internal static int BaseMagicalDamage(IWeapon weapon, Player player)
        => weapon.Damage + player.Wisdom;
}
