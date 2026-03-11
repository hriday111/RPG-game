namespace RpgGame.Tiles;

/// <summary>
/// Represents a solid wall tile that blocks player movement.
/// </summary>
/// <remarks>
/// Wall tiles are non-walkable obstacles that form barriers,
/// boundaries, and structural elements within the level.
/// </remarks>
public sealed class WallTile : Tile
{
    /// <summary>
    /// Gets the character symbol used to render the wall on the map.
    /// </summary>
    public override char Symbol => '█';

    public override ConsoleColor color => ConsoleColor.Gray;
    /// <summary>
    /// Gets a value indicating whether the wall is walkable.
    /// </summary>
    /// <remarks>
    /// Always returns false; walls block all movement.
    /// </remarks>
    public override bool IsWalkable => false;
}