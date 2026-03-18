using RpgGame.Generation.Procedures;

namespace RpgGame.Generation.Strategies;

/// <summary>
/// A concrete dungeon generation strategy that creates an empty dungeon with on central room, and items.
/// </summary>
/// <remarks>
/// This strategy uses the following pipeline:
/// 1. Fills the level with walls as a base
/// 2. Carves a central room in the middle
/// 3. Spawns coins and gold for the player to collect
/// 4. Spawns weapons for the player to find
/// </remarks>
public class DungeonSandboxStrategy : IDungeonStrategy
{
    /// <summary>
    /// Creates and returns a configured dungeon builder with the strategy's procedures.
    /// </summary>
    /// <returns>A fully configured dungeon builder ready to generate levels.</returns>
    public DungeonBuilder Create()
    {
        return new DungeonBuilder()
            .Add(new FilledDungeonProcedure())
            .Add(new CentralRoomProcedure(20, 10))
            .Add(new AddItemsProcedure(5, 2))
            .Add(new AddWeaponsProcedure());
    }
}
