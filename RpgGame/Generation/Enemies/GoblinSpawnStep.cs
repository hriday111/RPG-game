using RpgGame.Core;

namespace RpgGame.Generation.Enemies;

public sealed class GoblinSpawnStep : IEnemySpawnStep
{
    private readonly int count;

    public GoblinSpawnStep(int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        this.count = count;
    }

    public Task SpawnAsync(Level level) => MapSpawnHelper.SpawnGoblinAsync(level, count);
}
