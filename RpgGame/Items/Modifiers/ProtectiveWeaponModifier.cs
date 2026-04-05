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
    protected override void OnAfterEquippedToHands(Player player)
    {
        player.ApplyWisdomDelta(WisdomBonus);
        base.OnAfterEquippedToHands(player);
    }

    /// <inheritdoc />
    protected override void OnBeforeRemovedFromHands(Player player)
    {
        player.ApplyWisdomDelta(-WisdomBonus);
        base.OnBeforeRemovedFromHands(player);
    }
}
