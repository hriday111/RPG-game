using RpgGame.Core;
using RpgGame.Tiles;

namespace RpgGame.Generation.Procedures;

/// <summary>
/// Generates a central room in the dungeon.
/// </summary>
/// <remarks>
/// This procedure carves out a single rectangular room positioned at the center of the level.
/// The room is carved out as floor tiles, and its bounds are registered in the context for
/// later use by other procedures (e.g., path generation).
/// </remarks>
public class CentralRoomProcedure : IDungeonProcedure
{
    private readonly int width;
    private readonly int height;

    /// <summary>
    /// Initializes a central room procedure.
    /// </summary>
    /// <param name="width">The width of the central room in tiles.</param>
    /// <param name="height">The height of the central room in tiles.</param>
    public CentralRoomProcedure(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    /// <summary>
    /// Applies the central room generation to the level.
    /// </summary>
    /// <param name="level">The level being generated.</param>
    /// <param name="context">Shared generation state.</param>
    /// <returns>A completed task.</returns>
    public Task ApplyAsync(Level level, DungeonContext context)
    {
        int x = level.Width / 2 - width / 2;
        int y = level.Height / 2 - height / 2;

        var room = new RectRoom(x, y, width, height);
        context.Rooms.Add(room);

        for (int yy = y; yy < y + height; yy++)
        {
            for (int xx = x; xx < x + width; xx++)
            {
                level.SetTile(xx, yy, new FloorTile());
            }
        }

        return Task.CompletedTask;
    }
}