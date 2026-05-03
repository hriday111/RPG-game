using RpgGame.Generation;
using RpgGame.Generation.Procedures;
using RpgGame.Generation.Themes;

namespace RpgGame.Generation.Strategies;

/// <summary>
/// Treasury focus: large central vault, many side rooms, heavy coin/gold spawns, mages.
/// </summary>
public sealed class TreasureDungeonStrategy : IDungeonStrategy
{
    public DungeonBuilder Create()
    {
        DungeonThemeProfile p = DungeonThemeCatalog.Profile(DungeonThemeKind.Treasure);

        return new DungeonBuilder(DungeonThemeKind.Treasure)
            .Add(new FilledDungeonProcedure())
            .Add(new CentralRoomProcedure(14, 8))
            .Add(new ChambersProcedure(10))
            .Add(new PathsProcedure())
            .Add(new AddItemsProcedure(p.Items.Coins, p.Items.Gold, p.Items.Potions, p.Items.Thorns))
            .Add(new AddWeaponsProcedure(p.Weapons.Swords, p.Weapons.DoubleSwords, p.Weapons.CrystalOrbs))
            .Add(new PlaceArtifactProcedure(p.CreateArtifact))
            .Add(new AddEnemiesProcedure(p.EnemySpawnSteps));
    }
}
