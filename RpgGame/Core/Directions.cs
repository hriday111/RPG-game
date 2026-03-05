using RpgGame.Character;
namespace RpgGame.Core;

/// <summary>
/// Provides predefined directional offset vectors for common cardinal directions.
/// </summary>
public static class Directions
{
    public static readonly Position Up = new(0, -1);
    public static readonly Position Down = new(0, 1);
    public static readonly Position Left = new(-1, 0);
    public static readonly Position Right = new(1, 0);
}

