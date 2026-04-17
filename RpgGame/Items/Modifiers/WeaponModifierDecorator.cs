using RpgGame.Character;
using RpgGame.Combat;
using RpgGame.Core;

namespace RpgGame.Items.Modifiers;

/// <summary>
/// Decorator base for <see cref="Weapon"/> stacks: forwards most behavior to an inner
/// <see cref="IEquippable"/> while allowing subclasses to adjust name, damage, and
/// equip-side player stats. Does not add an effect list to weapon types.
/// </summary>
public abstract class WeaponModifierDecorator : IWeapon
{
    /// <summary>
    /// The wrapped item (another decorator or a concrete <see cref="Weapon"/>).
    /// </summary>
    protected IEquippable Inner { get; }

    protected WeaponModifierDecorator(IEquippable inner)
    {
        Inner = inner ?? throw new ArgumentNullException(nameof(inner));
    }

    /// <inheritdoc />
    public abstract string Name { get; }

    /// <inheritdoc />
    public virtual char Symbol => Inner.Symbol;

    /// <inheritdoc />
    public virtual ConsoleColor color => Inner.color;

    /// <inheritdoc />
    public virtual int Damage => ((IWeapon)Inner).Damage;

    /// <inheritdoc />
    public virtual int Defense => ((IWeapon)Inner).Defense;

    /// <inheritdoc />
    public virtual void AcceptCombatStrike(ICombatAttack attack, Player player, ICombatContribution contribution)
        => ResolveCoreWeapon().DispatchCombatUsingSurface(attack, this, player, contribution);

    /// <inheritdoc />
    public virtual void ContributeCombat(ICombatAttack attack, Player player, ICombatContribution contribution)
        => AcceptCombatStrike(attack, player, contribution);

    /// <inheritdoc />
    public virtual string GetDescription() => $"{Name} (Damage: {Damage})";

    /// <inheritdoc />
    public bool OnPickup(Player player, Inventory inventory)
        => inventory.AddToInventory(this);

    /// <inheritdoc />
    public void OnDrop(Level level, Player player)
    {
        OnRemovedFromHands(player);
        level.AddItem(player.Pos, this);
    }

    /// <inheritdoc />
    public bool TryEquipToLeft(Player player)
    {
        if (!ResolveCoreWeapon().ApplyEquipLeft(player, this))
            return false;
        OnEquippedToHands(player);
        return true;
    }

    /// <inheritdoc />
    public bool TryEquipToRight(Player player)
    {
        if (!ResolveCoreWeapon().ApplyEquipRight(player, this))
            return false;
        OnEquippedToHands(player);
        return true;
    }

    /// <summary>
    /// Walks the decorator chain to the concrete <see cref="Weapon"/> that owns hand rules.
    /// </summary>
    protected Weapon ResolveCoreWeapon()
    {
        return Inner switch
        {
            Weapon w => w,
            WeaponModifierDecorator d => d.ResolveCoreWeapon(),
            _ => throw new InvalidOperationException("Weapon decorator chain must end with a Weapon.")
        };
    }

    /// <summary>
    /// Invoked after this stack has been stored in the player's hands.
    /// </summary>
    public virtual void OnEquippedToHands(Player player) => Inner.OnEquippedToHands(player);

    /// <summary>
    /// Invoked when the item is dropped to the ground; reverses equip-time stat changes.
    /// </summary>
    public virtual void OnRemovedFromHands(Player player) => Inner.OnRemovedFromHands(player);
}
