using RpgGame.Character;

namespace RpgGame.Items.Modifiers;

/// <summary>
/// Decorator that reduces the player's luck by 5 while this stack is equipped.
/// Name gains an "(Unlucky)" suffix.
/// </summary>
public sealed class UnluckyWeaponModifier : WeaponModifierDecorator
{
    private const int LuckPenalty = 5;

    public UnluckyWeaponModifier(IEquippable inner)
        : base(inner)
    {
    }

    /// <inheritdoc />
    public override string Name => $"{Inner.Name} (Unlucky)";

    /// <inheritdoc />
    protected override void OnAfterEquippedToHands(Player player)
    {
        player.ApplyLuckDelta(-LuckPenalty);
        base.OnAfterEquippedToHands(player);
    }

    /// <inheritdoc />
    protected override void OnBeforeRemovedFromHands(Player player)
    {
        player.ApplyLuckDelta(LuckPenalty);
        base.OnBeforeRemovedFromHands(player);
    }
}
