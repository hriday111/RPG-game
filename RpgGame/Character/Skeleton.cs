using RpgGame.Character.Behavior;
using RpgGame.Core;
using RpgGame.Items;
using RpgGame.Logger;

namespace RpgGame.Character;

/// <summary>
/// Aggressive species: gets stronger when allies die.
/// </summary>
public sealed class Skeleton : Character, ISpeciesMember, INoiseListener
{
    public SpeciesKind Kind => SpeciesKind.Skeleton;

    public override char Symbol => 's';
    public IWeapon EquippedWeapon { get; }

    public int Health { get; private set; }
    public int Armor { get; }

    private int rageStacks;

    public Skeleton(Position startPosition, IWeapon equippedWeapon, int health = 26, int baseArmor = 1)
        : base(startPosition)
    {
        EquippedWeapon = equippedWeapon ?? throw new ArgumentNullException(nameof(equippedWeapon));
        Health = health;
        Armor = baseArmor + equippedWeapon.Defense;
    }

    public bool IsDead => Health <= 0;

    public int GetCounterAttackPower()
    {
        return EquippedWeapon.Damage + rageStacks;
    }

    public int ApplyDamage(int rawDamage)
    {
        int effectiveArmor = Armor + rageStacks;
        int dealt = Math.Max(0, rawDamage - effectiveArmor);

        Health -= dealt;
        if (Health < 0)
            Health = 0;

        return dealt;
    }

    public void OnSpeciesMemberDeath()
    {
        rageStacks++;
    }

    /// <inheritdoc />
    public Position ListenerTile => Pos;

    /// <inheritdoc />
    public void OnWeaponPickupNoise(Position soundSource, int graphDistanceSteps)
    {
        GameLog.Write(new EnemyHeardWeaponPickupNoiseLogEvent(Symbol, Pos, soundSource, graphDistanceSteps));
    }
}
