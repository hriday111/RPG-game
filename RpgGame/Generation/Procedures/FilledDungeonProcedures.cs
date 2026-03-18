using RpgGame.Core;
using RpgGame.Tiles;

namespace RpgGame.Generation.Procedures;

/// <summary>
/// Fills the entire dungeon with wall tiles, creating a solid base for carving.
/// </summary>
/// <remarks>
/// This is typically the first procedure in a dungeon generation pipeline, providing
/// a solid foundation that other procedures (like room carving) can modify.
/// </remarks>
public class FilledDungeonProcedure : IDungeonProcedure
{
    /// <summary>
    /// Applies the filled dungeon generation to the level.
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
                level.SetTile(x, y, new WallTile());
            }
        }

        return Task.CompletedTask;
    }
}
