using RpgGame.Core;
using RpgGame.Tiles;
using RpgGame.Character;
namespace RpgGame.Generation.Procedures;

/// <summary>
/// Generates corridors connecting all rooms to a central room.
/// </summary>
/// <remarks>
/// This procedure creates pathways from the central room to each additional room using
/// a simple L-shaped corridor algorithm (horizontal then vertical movement).
/// </remarks>
public class PathsProcedure : IDungeonProcedure
{
    /// <summary>
    /// Applies the corridor carving procedure to the level.
    /// </summary>
    /// <param name="level">The level being generated.</param>
    /// <param name="context">Shared generation state containing the rooms to connect.</param>
    /// <returns>A completed task.</returns>
    public Task ApplyAsync(Level level, DungeonContext context)
    {
        if (context.Rooms.Count < 2)
            return Task.CompletedTask;

        var centralRoom = context.Rooms[0];

        foreach (var room in context.Rooms.Skip(1))
        {
            CarveCorridor(level, centralRoom.Center, room.Center);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Carves an L-shaped corridor between two positions.
    /// </summary>
    /// <param name="level">The level to modify.</param>
    /// <param name="from">The starting position.</param>
    /// <param name="to">The destination position.</param>
    private void CarveCorridor(Level level, Position from, Position to)
    {
        var current = from;

        while (current.X != to.X)
        {
            level.SetTile(current.X, current.Y, new FloorTile());
            current = current + new Position(Math.Sign(to.X - current.X), 0);
        }

        while (current.Y != to.Y)
        {
            level.SetTile(current.X, current.Y, new FloorTile());
            current = current + new Position(0, Math.Sign(to.Y - current.Y));
        }
    }
}