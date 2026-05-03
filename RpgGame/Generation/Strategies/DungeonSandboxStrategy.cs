using RpgGame.Generation;
using RpgGame.Generation.Procedures;
using RpgGame.Generation.Themes;

namespace RpgGame.Generation.Strategies;

/// <summary>
/// Sandbox: one large chamber and themed loot, artifact, enemies from <see cref="DungeonThemeCatalog"/>.
/// </summary>
public class DungeonSandboxStrategy : IDungeonStrategy
{
    private readonly DungeonThemeKind theme;

    public DungeonSandboxStrategy(DungeonThemeKind theme = DungeonThemeKind.Basic)
    {
        this.theme = theme;
    }

    /// <summary>
    /// Creates and returns a configured dungeon builder with the strategy's procedures.
    /// </summary>
    /// <returns>A fully configured dungeon builder ready to generate levels.</returns>
    public DungeonBuilder Create()
    {
        DungeonThemeProfile p = DungeonThemeCatalog.Profile(theme);

        return new DungeonBuilder(theme)
            .Add(new FilledDungeonProcedure())
            .Add(new CentralRoomProcedure(20, 10))
            .Add(new AddItemsProcedure(p.Items.Coins, p.Items.Gold, p.Items.Potions, p.Items.Thorns))
            .Add(new AddWeaponsProcedure(p.Weapons.Swords, p.Weapons.DoubleSwords, p.Weapons.CrystalOrbs))
            .Add(new PlaceArtifactProcedure(p.CreateArtifact))
            .Add(new AddEnemiesProcedure(p.EnemySpawnSteps));
    }
}
