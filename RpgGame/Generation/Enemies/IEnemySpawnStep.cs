using RpgGame.Core;

namespace RpgGame.Generation.Enemies;

/// <summary>
/// One thematic enemy spawn contribution (one or several of the same enemy type via count).
/// Themes compose many steps in a list; <see cref="AddEnemiesProcedure"/> runs each in order.
/// </summary>
public interface IEnemySpawnStep
{
    Task SpawnAsync(Level level);
}
