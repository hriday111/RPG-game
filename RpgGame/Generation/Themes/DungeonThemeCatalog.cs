using System.Collections.Immutable;
using RpgGame.Generation.Enemies;
using RpgGame.Items;

namespace RpgGame.Generation.Themes;

/// <summary>Counts passed to <see cref="Procedures.AddItemsProcedure"/>.</summary>
public readonly record struct ThemeItemCounts(int Coins, int Gold, int Potions, int Thorns);

/// <summary>Counts passed to <see cref="Procedures.AddWeaponsProcedure"/>.</summary>
public readonly record struct ThemeWeaponCounts(int Swords, int DoubleSwords, int CrystalOrbs);

/// <summary>
/// All theme-tunable content: intro, loot mix, guaranteed artifact, and enemy spawn pipeline.
/// Layout lives in per-theme <see cref="Strategies.IDungeonStrategy"/> implementations.
/// </summary>
public sealed class DungeonThemeProfile
{
    public required string IntroMessage { get; init; }
    public required ThemeItemCounts Items { get; init; }
    public required ThemeWeaponCounts Weapons { get; init; }
    public required Func<IItem> CreateArtifact { get; init; }
    public required ImmutableArray<IEnemySpawnStep> EnemySpawnSteps { get; init; }
}

/// <summary>
/// Registry of theme profiles (no <c>switch</c> at use sites — dictionary lookup only).
/// </summary>
public static class DungeonThemeCatalog
{
    private static readonly ImmutableDictionary<DungeonThemeKind, DungeonThemeProfile> Profiles = Build();

    private static ImmutableDictionary<DungeonThemeKind, DungeonThemeProfile> Build()
    {
        var b = ImmutableDictionary.CreateBuilder<DungeonThemeKind, DungeonThemeProfile>();

        b.Add(DungeonThemeKind.Basic, new DungeonThemeProfile
        {
            IntroMessage = "Stone grinds on stone; something heavy stirs below.",
            Items = new ThemeItemCounts(5, 2, 2, 1),
            Weapons = new ThemeWeaponCounts(2, 1, 1),
            CreateArtifact = () => new Sword(),
            EnemySpawnSteps = ImmutableArray.Create<IEnemySpawnStep>(
                new GoblinSpawnStep(2),
                new SkeletonSpawnStep(2)),
        });

        b.Add(DungeonThemeKind.Treasure, new DungeonThemeProfile
        {
            IntroMessage = "You feel an itch in your wallet — glitter seeps from every crack.",
            Items = new ThemeItemCounts(18, 10, 0, 0),
            Weapons = new ThemeWeaponCounts(0, 0, 0),
            CreateArtifact = () => new Gold(),
            EnemySpawnSteps = ImmutableArray.Create<IEnemySpawnStep>(new MageSpawnStep(2)),
        });

        b.Add(DungeonThemeKind.Library, new DungeonThemeProfile
        {
            IntroMessage = "The smell of old bindings and dust clings to the corridors.",
            Items = new ThemeItemCounts(2, 1, 1, 0),
            Weapons = new ThemeWeaponCounts(1, 0, 4),
            CreateArtifact = () => new CrystalOrb(),
            EnemySpawnSteps = ImmutableArray.Create<IEnemySpawnStep>(new GolemSpawnStep(1)),
        });

        b.Add(DungeonThemeKind.Healing, new DungeonThemeProfile
        {
            IntroMessage = "Cool air and a faint herbal scent promise respite.",
            Items = new ThemeItemCounts(2, 1, 12, 0),
            Weapons = new ThemeWeaponCounts(1, 0, 1),
            CreateArtifact = () => new Potion(),
            EnemySpawnSteps = ImmutableArray.Create<IEnemySpawnStep>(new GolemSpawnStep(1)),
        });

        return b.ToImmutable();
    }

    public static DungeonThemeProfile Profile(DungeonThemeKind theme) =>
        Profiles.TryGetValue(theme, out DungeonThemeProfile? p)
            ? p
            : Profiles[DungeonThemeKind.Basic];
}
