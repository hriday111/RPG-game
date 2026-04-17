using RpgGame.Character;

namespace RpgGame.Items.Modifiers;

/// <summary>
/// Decorator that increases the player's wisdom by 5 while this stack is equipped.
/// Name gains a "(Protective)" suffix.
/// </summary>
public sealed class ProtectiveWeaponModifier : WeaponModifierDecorator
{
    private const int WisdomBonus = 5;

    public ProtectiveWeaponModifier(IEquippable inner)
        : base(inner)
    {
    }

    /// <inheritdoc />
    public override string Name => $"{Inner.Name} (Protective)";

    /// <inheritdoc />
    public override int Defense => base.Defense + 3;

    /// <inheritdoc />
    public override void OnEquippedToHands(Player player)
    {
        player.ApplyWisdomDelta(WisdomBonus);
        base.OnEquippedToHands(player);
    }

    /// <inheritdoc />
    public override void OnRemovedFromHands(Player player)
    {
        player.ApplyWisdomDelta(-WisdomBonus);
        base.OnRemovedFromHands(player);
    }
}
