using System.Diagnostics.Contracts;
using RpgGame.Core;
namespace RpgGame.Tiles;

/// <summary>
/// Base abstract class representing a tile in the game world.
/// </summary>
/// <remarks>
/// Tiles form the foundation of the level terrain. Each tile defines
/// whether it can be walked on and how it should be rendered on the map.
/// 
/// Concrete implementations (such as <see cref="FloorTile"/> and
/// <see cref="WallTile"/>) must provide their own symbol and walkability
/// characteristics.
/// </remarks>
public abstract class Tile
{
    /// <summary>
    /// Gets the character symbol used to render the tile on the map.
    /// </summary>
    public abstract char Symbol { get; }

    public abstract ConsoleColor color {get;}
    /// <summary>
    /// Gets a value indicating whether characters can walk on this tile.
    /// </summary>
    public abstract bool IsWalkable { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the tile is currently occupied by a character or entity.
    /// </summary>
    public bool IsOccupied{get; set;}

    /// <summary>
    /// Internal list used for position tracking. Reserved for future tile-related operations.
    /// </summary>
    public   List<Tile> PosList = new List<Tile>((Config.WindowHeight-1)*(Config.WindowHeight-1));

}