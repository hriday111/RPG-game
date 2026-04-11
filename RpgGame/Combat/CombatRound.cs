using RpgGame.Character;

namespace RpgGame.Combat;

/// <summary>
/// Outcome of one combat exchange: player strikes, then living enemies strike back.
/// </summary>
public readonly record struct CombatRoundResult(
    bool EnemyDefeated,
    int PlayerRawDamage,
    int DamageAppliedToEnemy,
    int DamageAppliedToPlayer);

/// <summary>
/// Resolves a single turn of melee combat between the player and one golem.
/// </summary>
public static class CombatRound
{
    /// <summary>
    /// Player attacks first. If the golem survives, it counter-attacks once.
    /// Enemy damage is reduced by the player's <see cref="Player.GetDefenseStrength"/>.
    /// </summary>
    public static CombatRoundResult Resolve(Player player, Golem golem)
    {
        int raw = player.GetMeleeAttackPower();
        int toEnemy = golem.ApplyDamage(raw);

        if (golem.IsDead)
            return new CombatRoundResult(true, raw, toEnemy, 0);

        int toPlayer = Math.Max(0, golem.GetCounterAttackPower() - player.GetDefenseStrength());
        player.TakeDamage(toPlayer);
        return new CombatRoundResult(false, raw, toEnemy, toPlayer);
    }
}
