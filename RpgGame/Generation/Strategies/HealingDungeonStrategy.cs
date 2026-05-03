using RpgGame.Generation;
using RpgGame.Generation.Procedures;
using RpgGame.Generation.Themes;

namespace RpgGame.Generation.Strategies;

/// <summary>
/// Calm layout with moderate chambers; loot profile emphasizes potions and a restorative artifact.
/// </summary>
public sealed class HealingDungeonStrategy : IDungeonStrategy
{
    public DungeonBuilder Create()
    {
        DungeonThemeProfile p = DungeonThemeCatalog.Profile(DungeonThemeKind.Healing);

        return new DungeonBuilder(DungeonThemeKind.Healing)
            .Add(new FilledDungeonProcedure())
            .Add(new CentralRoomProcedure(9, 6))
            .Add(new ChambersProcedure(7))
            .Add(new PathsProcedure())
            .Add(new AddItemsProcedure(p.Items.Coins, p.Items.Gold, p.Items.Potions, p.Items.Thorns))
            .Add(new AddWeaponsProcedure(p.Weapons.Swords, p.Weapons.DoubleSwords, p.Weapons.CrystalOrbs))
            .Add(new PlaceArtifactProcedure(p.CreateArtifact))
            .Add(new AddEnemiesProcedure(p.EnemySpawnSteps));
    }
}
