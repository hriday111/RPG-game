using RpgGame.Core;

namespace RpgGame.Generation.Enemies;

public sealed class SkeletonSpawnStep : IEnemySpawnStep
{
    private readonly int count;

    public SkeletonSpawnStep(int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        this.count = count;
    }

    public Task SpawnAsync(Level level) => MapSpawnHelper.SpawnSkeletonAsync(level, count);
}
