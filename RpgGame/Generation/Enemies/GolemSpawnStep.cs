using RpgGame.Core;

namespace RpgGame.Generation.Enemies;

/// <summary>
/// Spawns a fixed number of golems.
/// </summary>
public sealed class GolemSpawnStep : IEnemySpawnStep
{
    private readonly int count;

    public GolemSpawnStep(int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        this.count = count;
    }

    /// <inheritdoc />
    public Task SpawnAsync(Level level) => MapSpawnHelper.SpawnGolemAsync(level, count);
}
