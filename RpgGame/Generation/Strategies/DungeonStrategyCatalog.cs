using System.Collections.Immutable;
using RpgGame.Generation.Themes;

namespace RpgGame.Generation.Strategies;

/// <summary>
/// Resolves a concrete <see cref="IDungeonStrategy"/> for each <see cref="DungeonThemeKind"/>
/// (<c>switch</c>-free lookup table).
/// </summary>
public static class DungeonStrategyCatalog
{
    private static readonly ImmutableDictionary<DungeonThemeKind, Func<IDungeonStrategy>> Strategies = Build();

    private static ImmutableDictionary<DungeonThemeKind, Func<IDungeonStrategy>> Build()
    {
        var b = ImmutableDictionary.CreateBuilder<DungeonThemeKind, Func<IDungeonStrategy>>();

        b.Add(DungeonThemeKind.Basic, static () => new BasicDungeonStrategy());
        b.Add(DungeonThemeKind.Treasure, static () => new TreasureDungeonStrategy());
        b.Add(DungeonThemeKind.Library, static () => new LibraryDungeonStrategy());
        b.Add(DungeonThemeKind.Healing, static () => new HealingDungeonStrategy());

        return b.ToImmutable();
    }

    public static IDungeonStrategy Resolve(DungeonThemeKind theme) =>
        Strategies.TryGetValue(theme, out Func<IDungeonStrategy>? factory)
            ? factory()
            : Strategies[DungeonThemeKind.Basic]();
}
