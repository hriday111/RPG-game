namespace RpgGame.Core;

/// <summary>
/// Represents runtime configuration values loaded for the game session.
/// </summary>
public sealed class GameConfig
{
    /// <summary>
    /// Gets the width of the map rendering area in characters.
    /// </summary>
    public int WindowWidth { get; init; }

    /// <summary>
    /// Gets the height of the map rendering area in characters.
    /// </summary>
    public int WindowHeight { get; init; }

    /// <summary>
    /// Gets the target number of frames per second for the main loop.
    /// </summary>
    public int TargetFPS { get; init; }

    /// <summary>
    /// Gets the player's initial X coordinate.
    /// </summary>
    public int DefaultSpawnX { get; init; }

    /// <summary>
    /// Gets the player's initial Y coordinate.
    /// </summary>
    public int DefaultSpawnY { get; init; }

    /// <summary>
    /// Gets the width of the right-side information panel.
    /// </summary>
    public int SidebarWidth { get; init; }

    /// <summary>
    /// Gets the player name used by systems such as log file naming.
    /// </summary>
    public string PlayerName { get; init; } = "Hero";

    /// <summary>
    /// Gets the directory where game logs are stored.
    /// </summary>
    public string LogDirectory { get; init; } = "./logs";
}
