using RpgGame.Core;
using RpgGame.Items;
using RpgGame.Logger;

namespace RpgGame.Character;

/// <summary>
/// Represents a mage enemy on the level. Uses the same melee/counter rules as <see cref="Golem"/>.
/// </summary>
public class Mage : Character, INoiseListener
{
    public override char Symbol => 'A';

    /// <summary>Weapon this mage fights with; <see cref="IWeapon.Damage"/> is used for counter-attacks.</summary>
    public IWeapon EquippedWeapon { get; }

    /// <summary>Current hit points; at 0 the mage should be removed from the level.</summary>
    public int Health { get; private set; }

    /// <summary>
    /// Damage reduction from incoming player hits: <paramref name="baseArmor"/> plus <see cref="IWeapon.Defense"/> from <see cref="EquippedWeapon"/>.
    /// </summary>
    public int Armor { get; }

    /// <param name="baseArmor">Armor before adding the weapon’s defensive value.</param>
    public Mage(Position startPosition, IWeapon equippedWeapon, int health = 24, int baseArmor = 1)
        : base(startPosition)
    {
        EquippedWeapon = equippedWeapon ?? throw new ArgumentNullException(nameof(equippedWeapon));
        Health = health;
        Armor = baseArmor + equippedWeapon.Defense;
    }

    /// <summary>True after <see cref="ApplyDamage"/> reduces <see cref="Health"/> to 0.</summary>
    public bool IsDead => Health <= 0;

    /// <summary>Raw damage for one counter-attack (before the player’s defense).</summary>
    public int GetCounterAttackPower() => EquippedWeapon.Damage;

    /// <summary>
    /// Applies damage reduced by <see cref="Armor"/>. Returns hit points actually removed.
    /// </summary>
    public int ApplyDamage(int rawDamage)
    {
        int dealt = Math.Max(0, rawDamage - Armor);
        Health -= dealt;
        if (Health < 0)
            Health = 0;
        return dealt;
    }

    /// <inheritdoc />
    public Position ListenerTile => Pos;

    /// <inheritdoc />
    public void OnWeaponPickupNoise(Position soundSource, int graphDistanceSteps)
    {
        GameLog.Write(new EnemyHeardWeaponPickupNoiseLogEvent(Symbol, Pos, soundSource, graphDistanceSteps));
    }
}
