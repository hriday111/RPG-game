using RpgGame.Character;
using RpgGame.Combat;
using RpgGame.Core;
using RpgGame.Items;
using RpgGame.Items.Modifiers;
using RpgGame.Tiles;

namespace RpgGame.Tests;

public class CombatRoundTests
{
    /// <summary>Test-only weapon with explicit damage/defense (golems use real <see cref="IWeapon"/> stats).</summary>
    private sealed class TunedBlade : Weapon
    {
        private readonly int _damage;
        private readonly int _weaponDefense;
        private readonly IWeaponCategory _category;

        public TunedBlade(int damage, int weaponDefense = 0, IWeaponCategory? category = null)
            : base(new OneHandOccupation())
        {
            _damage = damage;
            _weaponDefense = weaponDefense;
            _category = category ?? HeavyWeaponCategory.Instance;
        }

        protected override IWeaponCategory CombatCategory => _category;

        public override string Name => "Tuned blade";
        public override ConsoleColor color => ConsoleColor.Gray;
        public override char Symbol => 't';
        public override int Damage => _damage;
        public override int Defense => _weaponDefense;
    }

    /// <summary>Equippable that is not a <see cref="IWeapon"/> (combat damage 0 from that hand).</summary>
    private sealed class Trinket : IEquippable
    {
        private static readonly OneHandOccupation Occ = new();

        public string Name => "Trinket";
        public char Symbol => '%';
        public ConsoleColor color => ConsoleColor.Magenta;
        public string GetDescription() => "non-weapon";

        public bool OnPickup(Player player, Inventory inventory) => inventory.AddToInventory(this);

        public void OnDrop(Level level, Player player) => level.AddItem(player.Pos, this);

        public bool TryEquipToLeft(Player player) => Occ.EquipLeft(player, this);

        public bool TryEquipToRight(Player player) => Occ.EquipRight(player, this);
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
        Assert.Equal(4, result.DamageAppliedToPlayer);
        Assert.Equal(hpBefore - 4, player.Health);
    }

    [Fact]
    public void Resolve_Mage_DefeatsBeforeCounter_NoDamageToPlayer()
    {
        var player = new Player(new Position(0, 0));
        player.EquipLeft(new Sword());
        var mage = new Mage(new Position(1, 0), new TunedBlade(40), health: 6, baseArmor: 0);

        CombatRoundResult result = CombatRound.Resolve(player, mage);

        Assert.True(result.EnemyDefeated);
        Assert.Equal(0, result.DamageAppliedToPlayer);
        Assert.True(mage.IsDead);
    }

    [Fact]
    public void Resolve_EnemyArmor_ReducesDamageToEnemy()
    {
        var player = new Player(new Position(0, 0));
        player.EquipLeft(new Sword());
        var golem = new Golem(new Position(1, 0), new Sword(), health: 100, baseArmor: 8);

        CombatRoundResult result = CombatRound.Resolve(player, golem);

        Assert.Equal(10, result.DamageAppliedToEnemy);
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

    [Fact]
    public void LightWeapon_StealthAttack_DoublesCategoryDamage()
    {
        var player = new Player(new Position(0, 0));
        player.EquipLeft(new Sword());
        player.SetSelectedCombatAttack(StealthAttack.Instance);
        // (10 + DEX 5 + LCK 3) * 2
        Assert.Equal(36, player.GetOutgoingDamageForAttack(StealthAttack.Instance));
    }

    [Fact]
    public void MagicalWeapon_MagicalStrike_UsesWisdomScaling()
    {
        var player = new Player(new Position(0, 0));
        player.EquipLeft(new CrystalOrb());
        player.SetSelectedCombatAttack(MagicalStrikeAttack.Instance);
        // 8 + WIS 6
        Assert.Equal(14, player.GetOutgoingDamageForAttack(MagicalStrikeAttack.Instance));
    }

    [Fact]
    public void StrongModifier_ForwardsCategory_LightStealthUsesDecoratedDamage()
    {
        var player = new Player(new Position(0, 0));
        player.EquipLeft(new StrongWeaponModifier(new Sword()));
        player.SetSelectedCombatAttack(StealthAttack.Instance);
        // (15 + 5 + 3) * 2
        Assert.Equal(46, player.GetOutgoingDamageForAttack(StealthAttack.Instance));
    }

    [Fact]
    public void NonWeaponEquippable_DealsNoDamage()
    {
        var player = new Player(new Position(0, 0));
        player.EquipLeft(new Trinket());
        Assert.Equal(0, player.GetOutgoingDamageForAttack(NormalAttack.Instance));
    }

    [Fact]
    public void NormalAttack_MagicalWeapon_DealsOneDamage()
    {
        var player = new Player(new Position(0, 0));
        player.EquipLeft(new CrystalOrb());
        Assert.Equal(1, player.GetOutgoingDamageForAttack(NormalAttack.Instance));
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

    [Fact]
    public void TryGetMageAt_ReturnsTrueWhenMagePresent()
    {
        var level = new Level(8, 8);
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
                level.SetTile(x, y, new FloorTile());
        }

        var mage = new Mage(new Position(4, 4), new Sword());
        level.AddMage(mage);

        bool found = level.TryGetMageAt(new Position(4, 4), out Mage? atPos);

        Assert.True(found);
        Assert.Same(mage, atPos);
    }

    [Fact]
    public void TryOrthogonalStepOrMeleeCombat_Mage_SetsLastCombatMessage()
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
        level.AddMage(new Mage(new Position(2, 1), new Sword(), health: 40, baseArmor: 0));

        level.TryOrthogonalStepOrMeleeCombat(player, new Position(2, 1));

        Assert.False(string.IsNullOrEmpty(level.LastCombatMessage));
        Assert.Contains("mage", level.LastCombatMessage, StringComparison.OrdinalIgnoreCase);
    }
}
