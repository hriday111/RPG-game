namespace RpgGame.Generation.Strategies;

/// <summary>
/// Defines the contract for dungeon generation strategies.
/// </summary>
/// <remarks>
/// Implementations of this interface encapsulate complete dungeon generation pipelines,
/// allowing different dungeon layouts to be created using the Strategy Pattern.
/// Each strategy composes multiple <see cref="IDungeonProcedure"/> instances into a
/// <see cref="DungeonBuilder"/> to produce unique dungeon configurations.
/// </remarks>
public interface IDungeonStrategy
{
    /// <summary>
    /// Creates and returns a configured dungeon builder for this strategy.
    /// </summary>
    /// <returns>A dungeon builder with all procedures for this strategy already configured.</returns>
    DungeonBuilder Create();
}