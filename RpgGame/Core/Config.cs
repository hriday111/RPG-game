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
    public static GameConfig Current { get; private set; } = new GameConfig
    {
        WindowWidth = 40,
        WindowHeight = 20,
        TargetFPS = 60,
        DefaultSpawnX = 20,
        DefaultSpawnY = 10,
        SidebarWidth = 60,
        PlayerName = "Hero",
        LogDirectory = "./logs"
    };
    public static int WindowWidth => Current.WindowWidth;
    public static int WindowHeight => Current.WindowHeight;
    public static int TargetFPS => Current.TargetFPS;
    public static int DefaultSpawnX => Current.DefaultSpawnX;
    public static int DefaultSpawnY => Current.DefaultSpawnY;
    public static int SidebarWidth => Current.SidebarWidth;
    public static string PlayerName => Current.PlayerName;
    public static string LogDirectory => Current.LogDirectory;
    public static void Initialize(GameConfig config)
    {
        Current = config ?? throw new ArgumentNullException(nameof(config));
    }
}
