using RpgGame.Core;
using RpgGame.Tiles;

namespace RpgGame.Generation.Procedures;

/// <summary>
/// Generates multiple random chamber rooms in the dungeon.
/// </summary>
/// <remarks>
/// This procedure attempts to create random rectangular rooms that do not overlap with
/// existing rooms. Rooms are carved out as floor tiles and registered for later coordination.
/// </remarks>
public class ChambersProcedure : IDungeonProcedure
{
    private readonly int roomCount;
    private readonly Random random = new();

    /// <summary>
    /// Initializes a chambers generation procedure.
    /// </summary>
    /// <param name="roomCount">The maximum number of chambers to attempt to generate.</param>
    public ChambersProcedure(int roomCount)
    {
        this.roomCount = roomCount;
    }

    /// <summary>
    /// Applies the chambers generation to the level.
    /// </summary>
    /// <param name="level">The level being generated.</param>
    /// <param name="context">Shared generation state.</param>
    /// <returns>A completed task.</returns>
    public Task ApplyAsync(Level level, DungeonContext context)
    {
        for (int i = 0; i < roomCount; i++)
        {
            int tries = 0;
            bool roomPlaced = false;

            while (tries < 100000 && !roomPlaced)
            {
                int width = random.Next(4, 8);
                int height = random.Next(3, 6);

                int x = random.Next(2, level.Width - width - 2);
                int y = random.Next(2, level.Height - height - 2);

                var newRoom = new RectRoom(x, y, width, height);

                if (context.Rooms.Any(r => r.Intersects(newRoom)))
                {
                    tries++;
                    continue;
                }

                context.Rooms.Add(newRoom);

                for (int yy = y; yy < y + height; yy++)
                {
                    for (int xx = x; xx < x + width; xx++)
                    {
                        level.SetTile(xx, yy, new FloorTile());
                    }
                }
                roomPlaced = true;
            }
        }

        return Task.CompletedTask;
    }
}
