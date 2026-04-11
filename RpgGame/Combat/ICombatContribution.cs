namespace RpgGame.Combat;

/// <summary>
/// Collects per-hand damage and defense contributions for one combat exchange.
/// </summary>
public interface ICombatContribution
{
    void AddDamage(int amount);

    void AddDefense(int amount);
}
