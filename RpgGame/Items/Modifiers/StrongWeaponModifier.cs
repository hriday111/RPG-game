namespace RpgGame.Items.Modifiers;

/// <summary>
/// Decorator that increases weapon damage (+5). Name gains a "(Strong)" suffix.
/// </summary>
public sealed class StrongWeaponModifier : WeaponModifierDecorator
{
    public StrongWeaponModifier(IEquippable inner)
        : base(inner)
    {
    }

    /// <inheritdoc />
    public override string Name => $"{Inner.Name} (Strong)";

    /// <inheritdoc />
    public override int Damage => base.Damage + 5;
}
