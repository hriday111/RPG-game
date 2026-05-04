using RpgGame.Core;
using RpgGame.Generation.Themes;

namespace RpgGame.Generation;

/// <summary>
/// Implements the Builder Pattern to compose multiple dungeon generation procedures.
/// </summary>
/// <remarks>
/// This class orchestrates a sequence of <see cref="IDungeonProcedure"/> implementations,
/// allowing complex dungeon layouts to be built by chaining individual generation steps.
/// </remarks>
public class DungeonBuilder : IMapGenerator
{
    private readonly List<IDungeonProcedure> procedures = new();
    private readonly DungeonThemeKind theme;
    /// <summary>
    /// Adds a generation procedure to the sequence.
    /// </summary>
    /// <param name="procedure">The procedure to add.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public DungeonBuilder Add(IDungeonProcedure procedure)
    {
        procedures.Add(procedure);
        return this;
    }

    /// <summary>
    /// Asynchronously generates a dungeon by applying all registered procedures in sequence.
    /// </summary>
    /// <param name="level">The level instance to populate.</param>
    /// <returns>A task that completes when all procedures have been applied.</returns>
    public async Task GenerateAsync(Level level)
    {
        var context = new DungeonContext(theme);

        foreach (var procedure in procedures)
        {
            await procedure.ApplyAsync(level, context);
        }
    }

    public DungeonBuilder(DungeonThemeKind theme = DungeonThemeKind.Basic)
    {
        this.theme = theme;
    }
}
