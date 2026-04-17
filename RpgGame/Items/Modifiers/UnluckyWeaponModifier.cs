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
    public override void OnEquippedToHands(Player player)
    {
        player.ApplyLuckDelta(-LuckPenalty);
        base.OnEquippedToHands(player);
    }

    /// <inheritdoc />
    public override void OnRemovedFromHands(Player player)
    {
        player.ApplyLuckDelta(LuckPenalty);
        base.OnRemovedFromHands(player);
    }
}
