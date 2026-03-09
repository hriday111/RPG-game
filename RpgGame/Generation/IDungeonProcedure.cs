using RpgGame.Core;
using RpgGame.Tiles;
using RpgGame.Character;

namespace RpgGame.Generation;

/// <summary>
/// Defines a procedural step in dungeon generation.
/// </summary>
/// <remarks>
/// Implementations of this interface represent individual aspects of dungeon creation,
/// such as carving rooms, creating paths, or placing items. Multiple procedures can
/// be composed together via <see cref="DungeonBuilder"/> to create complex dungeons.
/// </remarks>
public interface IDungeonProcedure
{
    /// <summary>
    /// Asynchronously applies this generation step to the level.
    /// </summary>
    /// <param name="level">The level being generated.</param>
    /// <param name="context">Shared state for coordinating between procedures.</param>
    /// <returns>A task that completes when the procedure finishes.</returns>
    Task ApplyAsync(Level level, DungeonContext context);
}