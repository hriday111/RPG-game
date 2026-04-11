namespace RpgGame.Combat;

/// <summary>
/// Mutable accumulator used while visiting equipped items for a single strike.
/// </summary>
public sealed class CombatContributionAccumulator : ICombatContribution
{
    public int TotalDamage { get; private set; }

    public int TotalDefense { get; private set; }

    public void AddDamage(int amount) => TotalDamage += amount;

    public void AddDefense(int amount) => TotalDefense += amount;
}
