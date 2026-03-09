using RpgGame.Core;
using RpgGame.Tiles;
using RpgGame.Character;

namespace RpgGame.Generation;

/// <summary>
/// Generates a dungeon composed of multiple connected rooms.
/// </summary>
public class DungeonGenerator : IMapGenerator
{
    private record RectRoom(int X, int Y, int Width, int Height)
    {
        public Position Center => new(X + Width / 2, Y + Height / 2);

        public bool Intersects(RectRoom other)
        {
            return !(X + Width +2< other.X  ||
                     other.X + other.Width +2< X  ||
                     Y + Height +2< other.Y ||
                     other.Y + other.Height+2 < Y );
        }
    }
    private readonly Random random = new();

    public async Task GenerateAsync(Level level)
    {
        FillWithWalls(level);

        var rooms = new List<RectRoom>();

        // Create guaranteed central room
        var centerRoom = CreateCentralRoom(level);
        CarveRoom(level, centerRoom);
        rooms.Add(centerRoom);

        // Create additional rooms
        var additionalRooms = CreateRooms(level, 6);
        foreach (var room in additionalRooms)
        {
            CarveRoom(level, room);
            rooms.Add(room);
        }

        // Connect all rooms to central room
        foreach (var room in rooms.Where(r => r != centerRoom).ToList())
        {
            CarveCorridor(level, centerRoom.Center, room.Center);
        }

        // Add obstacles (skip central room!)
        AddRoomObstacles(level, rooms.Where(r => r != centerRoom).ToList());

        await MapSpawnHelper.SpawnCoinsAsync(level, 5);
        await MapSpawnHelper.SpawnSwordAsync(level, 2);
        await MapSpawnHelper.SpawnDoubleSwordAsync(level, 1);
        await MapSpawnHelper.SpawnGoldAsync(level, 2);
    }

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
    private void FillWithFloor(Level level)
    {
        for (int y = 1; y < level.Height-1; y++)
            {for (int x = 1; x < level.Width-1; x++)
                {level.SetTile(x, y, new WallTile());}}
    }
    private void FillWithWalls(Level level)
    {
        for (int y = 0; y < level.Height; y++)
            {for (int x = 0; x < level.Width; x++)
                {level.SetTile(x, y, new WallTile());}}
    }

    private RectRoom CreateCentralRoom(Level level)
    {
        int width = 10;
        int height = 6;

        int centerX = level.Width / 2 - width / 2;
        int centerY = level.Height / 2 - height / 2;

        return new RectRoom(centerX, centerY, width, height);
    }

    private List<RectRoom> CreateRooms(Level level, int count)
    {
        var rooms = new List<RectRoom>();

        for (int i = 0; i < count; i++)
        {
            const int maxAttemptsPerRoom = 100;  // prevent infinite loops
            int attempts = 0;
            bool placed = false;

            while (!placed && attempts < maxAttemptsPerRoom)
            {
                int width = random.Next(4, 8);  // vary sizes for more variety
                int height = random.Next(3, 6);

                int x = random.Next(1, level.Width - width - 1);
                int y = random.Next(1, level.Height - height - 1);

                var newRoom = new RectRoom(x, y, width, height);

                if (!rooms.Any(r => r.Intersects(newRoom)))
                {
                    rooms.Add(newRoom);
                    placed = true;
                }

                attempts++;
            }

            // if we couldn't place this room after max attempts, skip it
            // (the map might be too crowded)
        }

        return rooms;
    }

    private void CarveRoom(Level level, RectRoom room)
    {
        for (int y = room.Y; y < room.Y + room.Height; y++)
            {for (int x = room.X; x < room.X + room.Width; x++)
                {level.SetTile(x, y, new FloorTile());}}

    }

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

    private void AddRoomObstacles(Level level, List<RectRoom> rooms)
    {
        foreach (var room in rooms)
        {
            int obstacleCount = random.Next(1, 4);

            for (int i = 0; i < obstacleCount; i++)
            {
                int x = random.Next(room.X + 1, room.X + room.Width - 1);
                int y = random.Next(room.Y + 1, room.Y + room.Height - 1);

                level.SetTile(x, y, new WallTile());
            }
        }
    }

    

}