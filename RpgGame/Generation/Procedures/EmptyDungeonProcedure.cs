using RpgGame.Core;
using RpgGame.Tiles;

namespace RpgGame.Generation.Procedures;

/// <summary>
/// Fills the entire dungeon with floor tiles, creating an empty open space.
/// </summary>
/// <remarks>
/// This is a simple initialization procedure that can serve as a starting point for
/// more complex dungeon generation pipelines.
/// </remarks>
public class EmptyDungeonProcedure : IDungeonProcedure
{
    /// <summary>
    /// Applies the empty dungeon generation to the level.
    /// </summary>
    /// <param name="level">The level being generated.</param>
    /// <param name="context">Shared generation state.</param>
    /// <returns>A completed task.</returns>
    public Task ApplyAsync(Level level, DungeonContext context)
    {
        for (int y = 0; y < level.Height; y++)
        {
            for (int x = 0; x < level.Width; x++)
            {
                level.SetTile(x, y, new FloorTile());
            }
        }

        return Task.CompletedTask;
    }
}