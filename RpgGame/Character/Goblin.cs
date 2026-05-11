using RpgGame.Character.Behavior;
using RpgGame.Items;

namespace RpgGame.Character;

/// <summary>
/// Cowardly species: gets weaker when allies die.
/// </summary>
public sealed class Goblin : Character, ISpeciesMember
{
    public override char Symbol => 'g';
    public IWeapon EquippedWeapon { get; }

    public int Health { get; private set; }
    public int Armor { get; }

    private int panicStacks;
    public Goblin(Position startPosition, IWeapon equippedWeapon, int health = 22, int baseArmor = 1)
        : base(startPosition)
    {
        EquippedWeapon = equippedWeapon ?? throw new ArgumentNullException(nameof(equippedWeapon));
        Health = health;
        Armor = baseArmor + equippedWeapon.Defense;
    }

    public bool IsDead => Health <= 0;

    public int GetCounterAttackPower()
    {
        int weakened = EquippedWeapon.Damage - panicStacks;
        return Math.Max(1, weakened);
    }

    public int ApplyDamage(int rawDamage)
    {
        int effectiveArmor = Math.Max(0, Armor - panicStacks);
        int dealt = Math.Max(0, rawDamage - effectiveArmor);

        Health -= dealt;
        if (Health < 0)
            Health = 0;

        return dealt;
    }

    public void OnSpeciesMemberDeath()
    {
        panicStacks++;
    }
}
