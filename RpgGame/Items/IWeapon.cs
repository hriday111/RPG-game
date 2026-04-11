using RpgGame.Character;
using RpgGame.Combat;

namespace RpgGame.Items;

/// <summary>
/// An equippable item that contributes a damage value (typically a weapon).
/// </summary>
/// <remarks>
/// Used by weapon decorators to adjust damage without storing effect lists on
/// concrete weapon types.
/// </remarks>
public interface IWeapon : IEquippable
{
    /// <summary>
    /// Gets the effective damage for this weapon, including any stacked decorators.
    /// </summary>
    int Damage { get; }

    /// <summary>
    /// Defense contributed while this weapon is equipped (reduces incoming enemy damage).
    /// </summary>
    int Defense => 0;

    /// <summary>
    /// Double dispatch: weapon category forwards to <paramref name="attack"/>.
    /// </summary>
    void AcceptCombatStrike(ICombatAttack attack, Player player, ICombatContribution contribution);
}
