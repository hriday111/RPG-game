using RpgGame.Core;
using RpgGame.Tiles;
using RpgGame.Items;
using RpgGame.Character;
using System.Threading.Tasks;

namespace RpgGame.Generation;

/// <summary>
/// Generates a simple room-based level layout with borders,
/// random wall clusters, and predefined item spawns.
/// </summary>
/// <remarks>
/// The generated level consists of:
/// <list type="bullet">
/// <item><description>A fully walkable floor.</description></item>
/// <item><description>Solid border walls enclosing the map.</description></item>
/// <item><description>Random rectangular wall clusters as obstacles.</description></item>
/// <item><description>Randomly placed coins and weapons.</description></item>
/// </list>
/// 
/// This generator is intended as a basic implementation of
/// <see cref="IMapGenerator"/> and demonstrates procedural
/// level creation using randomized placement.
/// </remarks>
public class SimpleRoomGenerator : IMapGenerator
{
    /// <summary>
    /// Random number generator used for procedural placement.
    /// </summary>
    private readonly Random random = new();


    /// <summary>
    /// Asynchronously generates the level; uses the new async spawn helpers
    /// to perform item placement without blocking the caller.
    /// </summary>
    /// <param name="level">Level instance to populate.</param>
    public async Task GenerateAsync(Level level)
    {
        MakeFloor(level);
        MakeBorders(level);
        CreateRandomWallClusters(level);

        await MapSpawnHelper.SpawnCoinsAsync(level, 5);
        await MapSpawnHelper.SpawnSwordAsync(level, 4);
        await MapSpawnHelper.SpawnDoubleSwordAsync(level, 1);
        await MapSpawnHelper.SpawnGoldAsync(level, 2);
    }

    /// <summary>
    /// Fills the entire level with floor tiles.
    /// </summary>
    private void MakeFloor(Level level)
    {
        for (int y = 0; y < level.Height; y++)
        {
            for (int x = 0; x < level.Width; x++)
            {
                level.SetTile(x, y, new FloorTile());
            }
        }
    }

    /// <summary>
    /// Creates solid wall borders around the edges of the level.
    /// </summary>
    /// <remarks>
    /// Border tiles are marked as occupied to prevent movement.
    /// </remarks>
    private void MakeBorders(Level level)
    {
        for (int x = 0; x < level.Width; x++)
        {
            level.SetTile(x, 0, new WallTile());
            level.SetTile(x, level.Height - 1, new WallTile());

            level.GetTile(x, 0).IsOccupied = true;
            level.GetTile(x, level.Height - 1).IsOccupied = true;
        }

        for (int y = 0; y < level.Height; y++)
        {
            level.SetTile(0, y, new WallTile());
            level.SetTile(level.Width - 1, y, new WallTile());

            level.GetTile(0, y).IsOccupied = true;
            level.GetTile(level.Width - 1, y).IsOccupied = true;
        }
    }

    /// <summary>
    /// Creates random rectangular clusters of walls within the level.
    /// </summary>
    /// <remarks>
    /// Each cluster has a random starting position and random width and height.
    /// These clusters serve as obstacles inside the room.
    /// </remarks>
    private void CreateRandomWallClusters(Level level)
    {
        int clusterCount = 10;

        for (int i = 0; i < clusterCount; i++)
        {
            int startX = random.Next(3, level.Width - 3);
            int startY = random.Next(3, level.Height - 3);

            int clusterWidth = random.Next(2, 6);
            int clusterHeight = random.Next(2, 4);

            for (int y = startY; y < startY + clusterHeight && y < level.Height - 1; y++)
            {
                for (int x = startX; x < startX + clusterWidth && x < level.Width - 1; x++)
                {
                    level.SetTile(x, y, new WallTile());
                    level.GetTile(x, y).IsOccupied = true;
                }
            }
        }
    }

}