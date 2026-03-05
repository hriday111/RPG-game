namespace RpgGame.Tiles;

/// <summary>
/// Represents a walkable floor tile where players and items can exist.
/// </summary>
/// <remarks>
/// Floor tiles form the basic traversable surface of the level,
/// allowing characters to move freely across them and items to be placed.
/// </remarks>
public sealed class FloorTile : Tile
{
    /// <summary>
    /// Gets the character symbol used to render the floor on the map.
    /// </summary>
    public override char Symbol => ' ';

    /// <summary>
    /// Gets a value indicating whether the floor is walkable.
    /// </summary>
    /// <remarks>
    /// Always returns true; floor tiles are fully traversable.
    /// </remarks>
    public override bool IsWalkable => true;
}