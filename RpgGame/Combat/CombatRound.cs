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
    /// Damage and defense use <see cref="Player.SelectedCombatAttack"/> (F2–F4).
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

    /// <summary>Same exchange as <see cref="Resolve(Player, Golem)"/> for a mage.</summary>
    public static CombatRoundResult Resolve(Player player, Mage mage)
    {
        int raw = player.GetMeleeAttackPower();
        int toEnemy = mage.ApplyDamage(raw);

        if (mage.IsDead)
            return new CombatRoundResult(true, raw, toEnemy, 0);

        int toPlayer = Math.Max(0, mage.GetCounterAttackPower() - player.GetDefenseStrength());
        player.TakeDamage(toPlayer);
        return new CombatRoundResult(false, raw, toEnemy, toPlayer);
    }
    public static CombatRoundResult Resolve(Player player, Goblin goblin)
    {
        int raw = player.GetMeleeAttackPower();
        int toEnemy = goblin.ApplyDamage(raw);
        if (goblin.IsDead)
            return new CombatRoundResult(true, raw, toEnemy, 0);
        int toPlayer = Math.Max(0, goblin.GetCounterAttackPower() - player.GetDefenseStrength());
        player.TakeDamage(toPlayer);
        return new CombatRoundResult(false, raw, toEnemy, toPlayer);
    }
    public static CombatRoundResult Resolve(Player player, Skeleton skeleton)
    {
        int raw = player.GetMeleeAttackPower();
        int toEnemy = skeleton.ApplyDamage(raw);
        if (skeleton.IsDead)
            return new CombatRoundResult(true, raw, toEnemy, 0);
        int toPlayer = Math.Max(0, skeleton.GetCounterAttackPower() - player.GetDefenseStrength());
        player.TakeDamage(toPlayer);
        return new CombatRoundResult(false, raw, toEnemy, toPlayer);
    }
}
