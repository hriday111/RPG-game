using RpgGame.Character;
using RpgGame.Combat;
using RpgGame.Core;
using RpgGame.Items;
using RpgGame.Tiles;

namespace RpgGame.Tests;

public class CombatRoundTests
{
    /// <summary>Test-only weapon with explicit damage/defense (golems use real <see cref="IWeapon"/> stats).</summary>
    private sealed class TunedBlade : Weapon
    {
        private readonly int _damage;
        private readonly int _weaponDefense;

        public TunedBlade(int damage, int weaponDefense = 0)
            : base(new OneHandOccupation())
        {
            _damage = damage;
            _weaponDefense = weaponDefense;
        }

        public override string Name => "Tuned blade";
        public override ConsoleColor color => ConsoleColor.Gray;
        public override char Symbol => 't';
        public override int Damage => _damage;
        public override int Defense => _weaponDefense;
    }

    [Fact]
    public void Resolve_DefeatsEnemyBeforeCounter_NoDamageToPlayer()
    {
        var player = new Player(new Position(0, 0));
        player.EquipLeft(new Sword());
        var golem = new Golem(new Position(1, 0), new TunedBlade(50), health: 8, baseArmor: 0);

        CombatRoundResult result = CombatRound.Resolve(player, golem);

        Assert.True(result.EnemyDefeated);
        Assert.Equal(0, result.DamageAppliedToPlayer);
        Assert.True(golem.IsDead);
    }

    [Fact]
    public void Resolve_LivingEnemy_AppliesReducedDamageToPlayer()
    {
        var player = new Player(new Position(0, 0));
        player.EquipLeft(new Sword());
        var golem = new Golem(new Position(1, 0), new TunedBlade(12), health: 100, baseArmor: 0);
        int hpBefore = player.Health;

        CombatRoundResult result = CombatRound.Resolve(player, golem);

        Assert.False(result.EnemyDefeated);
        Assert.Equal(7, result.DamageAppliedToPlayer);
        Assert.Equal(hpBefore - 7, player.Health);
    }

    [Fact]
    public void Resolve_EnemyArmor_ReducesDamageToEnemy()
    {
        var player = new Player(new Position(0, 0));
        player.EquipLeft(new Sword());
        var golem = new Golem(new Position(1, 0), new Sword(), health: 100, baseArmor: 8);

        CombatRoundResult result = CombatRound.Resolve(player, golem);

        Assert.Equal(2, result.DamageAppliedToEnemy);
    }

    [Fact]
    public void Resolve_CounterDamage_ComesFromWeaponDamage()
    {
        var player = new Player(new Position(0, 0));
        var golem = new Golem(new Position(1, 0), new TunedBlade(20), health: 100, baseArmor: 0);

        CombatRound.Resolve(player, golem);

        int toPlayer = Math.Max(0, 20 - player.GetDefenseStrength());
        Assert.Equal(100 - toPlayer, player.Health);
    }
}

public class LevelCombatTests
{
    [Fact]
    public void TryGetGolemAt_ReturnsTrueWhenGolemPresent()
    {
        var level = new Level(8, 8);
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
                level.SetTile(x, y, new FloorTile());
        }

        var golem = new Golem(new Position(3, 3), new Sword());
        level.AddGolem(golem);

        bool found = level.TryGetGolemAt(new Position(3, 3), out Golem? atPos);

        Assert.True(found);
        Assert.Same(golem, atPos);
    }

    [Fact]
    public void TryOrthogonalStepOrMeleeCombat_SetsLastCombatMessage()
    {
        var level = new Level(6, 6);
        for (int y = 0; y < 6; y++)
        {
            for (int x = 0; x < 6; x++)
                level.SetTile(x, y, new FloorTile());
        }

        var player = new Player(new Position(2, 2));
        level.GetTile(2, 2).IsOccupied = true;
        player.EquipLeft(new Sword());
        level.AddGolem(new Golem(new Position(2, 1), new Sword(), health: 50, baseArmor: 0));

        level.TryOrthogonalStepOrMeleeCombat(player, new Position(2, 1));

        Assert.False(string.IsNullOrEmpty(level.LastCombatMessage));
        Assert.Contains("damage", level.LastCombatMessage, StringComparison.OrdinalIgnoreCase);
    }
}
