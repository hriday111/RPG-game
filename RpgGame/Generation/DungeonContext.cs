using RpgGame.Core;
namespace RpgGame.Generation;

/// <summary>
/// Provides shared state for dungeon generation procedures.
/// </summary>
/// <remarks>
/// This class maintains information about rooms and other features created during
/// the dungeon generation process, allowing procedures to coordinate their work.
/// </remarks>
public class DungeonContext
{
    /// <summary>
    /// Gets the collection of rooms created during dungeon generation.
    /// </summary>
    public List<RectRoom> Rooms { get; } = new();
}
