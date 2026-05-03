using System.Collections.Immutable;
using RpgGame.Core;
using RpgGame.Generation.Enemies;

namespace RpgGame.Generation.Procedures;

/// <summary>
/// Spawns enemies from an ordered list provided at construction — no branching on dungeon theme here.
/// </summary>
public sealed class AddEnemiesProcedure : IDungeonProcedure
{
    private readonly ImmutableArray<IEnemySpawnStep> enemySpawnSteps;

    /// <summary>
    /// Preferred when using <see cref="Themes.DungeonThemeProfile.EnemySpawnSteps"/>.
    /// </summary>
    public AddEnemiesProcedure(ImmutableArray<IEnemySpawnStep> enemySpawnSteps)
    {
        if (enemySpawnSteps.IsDefaultOrEmpty)
            throw new ArgumentException("At least one enemy spawn step is required.", nameof(enemySpawnSteps));

        this.enemySpawnSteps = enemySpawnSteps;
    }

    /// <summary>
    /// Accepts any read-only list (e.g. from tests or dynamic composition).
    /// </summary>
    public AddEnemiesProcedure(IReadOnlyList<IEnemySpawnStep> steps)
    {
        ArgumentNullException.ThrowIfNull(steps);
        if (steps.Count == 0)
            throw new ArgumentException("At least one enemy spawn step is required.", nameof(steps));

        enemySpawnSteps = ImmutableArray.CreateRange(steps);
    }

    public async Task ApplyAsync(Level level, DungeonContext context)
    {
        for (int i = 0; i < enemySpawnSteps.Length; i++)
        {
            await enemySpawnSteps[i].SpawnAsync(level);
        }
    }
}
