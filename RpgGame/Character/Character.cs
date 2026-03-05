
using System.Dynamic;
using System.Numerics;
using RpgGame.Core;
namespace RpgGame.Character;

/// <summary>
/// A record for tracking the position of Characters, Items, and Tiles if needed.
/// </summary>
/// <param name="X">The horizontal coordinate (0-based from left).</param>
/// <param name="Y">The vertical coordinate (0-based from top).</param>
/// <remarks>
/// Position is represented as a readonly record struct for immutability and value semantics.
/// Supports arithmetic operations for convenient coordinate manipulation.
/// </remarks>
public readonly record struct Position(int X, int Y)
{
    /// <summary>
    /// Returns a new Position offset by the specified amounts.
    /// </summary>
    /// <param name="dx">Horizontal offset.</param>
    /// <param name="dy">Vertical offset.</param>
    /// <returns>A new Position with the applied offset.</returns>
    public Position Offset(int dx, int dy)
        => new Position(X + dx, Y + dy);

    /// <summary>
    /// Adds two positions component-wise.
    /// </summary>
    public static Position operator +(Position left, Position right)
        => new Position(left.X + right.X, left.Y + right.Y);

    /// <summary>
    /// Subtracts one position from another component-wise.
    /// </summary>
    public static Position operator -(Position left, Position right)
        => new Position(left.X - right.X, left.Y - right.Y);
    
    /// <summary>
    /// Multiplies a position by a scalar value.
    /// </summary>
    public static Position operator *(Position pos, int scalar)
        => new Position(pos.X * scalar, pos.Y * scalar);
}   

/// <summary>
/// Base abstract class for both playable and non-playable characters.
/// </summary>
/// <remarks>
/// The Character class provides the foundation for all character types in the game.
/// Character-derived classes must implement the Symbol property to define how they
/// are rendered on the map. Position tracking and movement are handled by the base class.
/// </remarks>
public abstract class Character
{
    /// <summary>
    /// Gets the character symbol used to render this character on the map.
    /// </summary>
    public abstract char Symbol { get; }

    /// <summary>
    /// Gets or sets the current position of the character in the level.
    /// </summary>
    public Position Pos { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Character"/> class.
    /// </summary>
    /// <param name="startPosition">The starting position for this character.</param>
    protected Character(Position startPosition)
    {
        Pos = startPosition;
    }

    /// <summary>
    /// Moves the character to a new position.
    /// </summary>
    /// <param name="newPos">The new position to move to.</param>
    /// <remarks>
    /// This method does not validate whether the target position is walkable.
    /// Movement validation should be performed by the caller.
    /// </remarks>
    public void Move(Position newPos)
    {
        Pos = newPos;
        
    }
}