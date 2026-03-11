using RpgGame.Core;
using RpgGame.Character;
namespace RpgGame.Generation;

/// <summary>
/// Represents a rectangular room in a dungeon.
/// </summary>
/// <remarks>
/// This class is used during dungeon generation to track room boundaries and detect
/// collisions between generated rooms.
/// </remarks>
public class RectRoom
{
    /// <summary>
    /// Gets the x-coordinate of the room's top-left corner.
    /// </summary>
    public int X { get; }

    /// <summary>
    /// Gets the y-coordinate of the room's top-left corner.
    /// </summary>
    public int Y { get; }

    /// <summary>
    /// Gets the width of the room in tiles.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the height of the room in tiles.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Initializes a new rectangular room.
    /// </summary>
    /// <param name="x">The x-coordinate of the top-left corner.</param>
    /// <param name="y">The y-coordinate of the top-left corner.</param>
    /// <param name="width">The room width in tiles.</param>
    /// <param name="height">The room height in tiles.</param>
    public RectRoom(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Gets the center position of the room.
    /// </summary>
    public Position Center => new(X + Width / 2, Y + Height / 2);

    /// <summary>
    /// Determines whether this room intersects with another room.
    /// </summary>
    /// <param name="other">The room to check against.</param>
    /// <returns>True if the rooms intersect with a 2-tile buffer; otherwise, false.</returns>
    public bool Intersects(RectRoom other)
    {
        return !(X + Width  < other.X ||
                 other.X + other.Width  < X ||
                 Y + Height  < other.Y ||
                 other.Y + other.Height  < Y);
    }
}