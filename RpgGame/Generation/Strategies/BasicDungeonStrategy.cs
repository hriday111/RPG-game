using RpgGame.Generation;
using RpgGame.Generation.Procedures;
using RpgGame.Generation.Themes;

namespace RpgGame.Generation.Strategies;

/// <summary>
/// Default complex dungeon: balanced central hall, several side chambers, standard loot profile.
/// </summary>
public sealed class BasicDungeonStrategy : IDungeonStrategy
{
    public DungeonBuilder Create()
    {
        DungeonThemeProfile p = DungeonThemeCatalog.Profile(DungeonThemeKind.Basic);

        return new DungeonBuilder(DungeonThemeKind.Basic)
            .Add(new FilledDungeonProcedure())
            .Add(new CentralRoomProcedure(10, 6))
            .Add(new ChambersProcedure(8))
            .Add(new PathsProcedure())
            .Add(new AddItemsProcedure(p.Items.Coins, p.Items.Gold, p.Items.Potions, p.Items.Thorns))
            .Add(new AddWeaponsProcedure(p.Weapons.Swords, p.Weapons.DoubleSwords, p.Weapons.CrystalOrbs))
            .Add(new PlaceArtifactProcedure(p.CreateArtifact))
            .Add(new AddEnemiesProcedure(p.EnemySpawnSteps));
    }
}
