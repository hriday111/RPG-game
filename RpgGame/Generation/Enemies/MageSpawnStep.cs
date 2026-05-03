using RpgGame.Core;

namespace RpgGame.Generation.Enemies;

/// <summary>
/// Spawns a fixed number of mages.
/// </summary>
public sealed class MageSpawnStep : IEnemySpawnStep
{
    private readonly int count;

    public MageSpawnStep(int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        this.count = count;
    }

    /// <inheritdoc />
    public Task SpawnAsync(Level level) => MapSpawnHelper.SpawnMageAsync(level, count);
}
