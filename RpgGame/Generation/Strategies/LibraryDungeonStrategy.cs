using RpgGame.Generation;
using RpgGame.Generation.Procedures;
using RpgGame.Generation.Themes;

namespace RpgGame.Generation.Strategies;

/// <summary>
/// Corridor-heavy scholastic ruin: smaller core, more chambers and paths, orb-weighted loot.
/// </summary>
public sealed class LibraryDungeonStrategy : IDungeonStrategy
{
    public DungeonBuilder Create()
    {
        DungeonThemeProfile p = DungeonThemeCatalog.Profile(DungeonThemeKind.Library);

        return new DungeonBuilder(DungeonThemeKind.Library)
            .Add(new FilledDungeonProcedure())
            .Add(new CentralRoomProcedure(8, 5))
            .Add(new ChambersProcedure(12))
            .Add(new PathsProcedure())
            .Add(new AddItemsProcedure(p.Items.Coins, p.Items.Gold, p.Items.Potions, p.Items.Thorns))
            .Add(new AddWeaponsProcedure(p.Weapons.Swords, p.Weapons.DoubleSwords, p.Weapons.CrystalOrbs))
            .Add(new PlaceArtifactProcedure(p.CreateArtifact))
            .Add(new AddEnemiesProcedure(p.EnemySpawnSteps));
    }
}
