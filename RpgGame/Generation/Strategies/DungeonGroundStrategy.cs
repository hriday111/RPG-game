using RpgGame.Generation.Procedures;

namespace RpgGame.Generation.Strategies;

/// <summary>
/// A concrete dungeon generation strategy that creates a complex dungeon with rooms, corridors, and items.
/// </summary>
/// <remarks>
/// This strategy uses the following pipeline:
/// 1. Fills the level with walls as a base
/// 2. Carves a central room in the middle
/// 3. Generates additional random chamber rooms
/// 4. Creates corridors connecting all rooms
/// 5. Spawns coins and gold for the player to collect
/// 6. Spawns weapons for the player to find
/// </remarks>
public class DungeonGroundsStrategy : IDungeonStrategy
{
    /// <summary>
    /// Creates and returns a configured dungeon builder with the strategy's procedures.
    /// </summary>
    /// <returns>A fully configured dungeon builder ready to generate levels.</returns>
    public DungeonBuilder Create()
    {
        return new DungeonBuilder()
            .Add(new FilledDungeonProcedure())
            .Add(new CentralRoomProcedure(10, 6))
            .Add(new ChambersProcedure(4))
            .Add(new PathsProcedure())
            .Add(new AddItemsProcedure(5, 2))
            .Add(new AddWeaponsProcedure());
    }
}