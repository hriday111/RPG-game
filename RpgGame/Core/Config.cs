namespace RpgGame.Core;

/// <summary>
/// Provides global configuration values for the game.
/// </summary>
/// <remarks>
/// This static class centralizes window, rendering, and default gameplay
/// parameters to avoid hardcoded "magic numbers" throughout the project.
/// 
/// Values are defined as constants because they are compile-time fixed
/// and represent immutable configuration settings.
/// </remarks>
public static class Config
{
    #region Game Window Settings

    /// <summary>
    /// Width of the main game map in characters.
    /// </summary>
    public const int WindowWidth = 40;

    /// <summary>
    /// Height of the main game map in characters.
    /// </summary>
    public const int WindowHeight = 20;

    /// <summary>
    /// Target frames per second for the game loop.
    /// </summary>
    /// <remarks>
    /// Used to approximate frame timing in the console loop.
    /// </remarks>
    public const int TargetFPS = 60;

    #endregion

    #region Player Settings

    /// <summary>
    /// Default horizontal spawn position for the player.
    /// </summary>
    public const int DefaultSpawnX = WindowWidth / 2;

    /// <summary>
    /// Default vertical spawn position for the player.
    /// </summary>
    public const int DefaultSpawnY = WindowHeight / 2;

    #endregion

    #region Sidebar Settings

    /// <summary>
    /// Width of the sidebar displaying player information and inventory.
    /// </summary>
    public const int SidebarWidth = 35;

    #endregion
}