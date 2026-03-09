using RpgGame.Core;

namespace RpgGame.Generation;

/// <summary>
/// Defines the contract for procedural level generation strategies.
/// </summary>
/// <remarks>
/// Implementations of this interface are responsible for constructing
/// the terrain layout of a <see cref="Level"/> instance.
/// 
/// This abstraction enables the use of the Strategy Pattern,
/// allowing different generation algorithms (e.g., simple rooms,
/// mazes, dungeon layouts) to be swapped without modifying the
/// game logic.
/// </remarks>
public interface IMapGenerator
{
    /// <summary>
    /// Asynchronously generates the terrain layout for the specified level.
    /// </summary>
    /// <param name="level">The level instance to populate.</param>
    /// <returns>A task that completes when generation is finished.</returns>
    /// <remarks>
    /// Implementations should perform all work asynchronously; callers may
    /// await the returned task to remain responsive during map creation.
    /// </remarks>
    Task GenerateAsync(Level level);
}