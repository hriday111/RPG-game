using System;
using System.Threading;
using System.Threading.Tasks;
using RpgGame.Character;
using RpgGame.Core;
using RpgGame.Generation.Strategies;
using RpgGame.Generation.Themes;
using RpgGame.Input;
using RpgGame.Logger;
using RpgGame.Renderer;
namespace RpgGame;

/// <summary>
/// Entry point of the RPG game application.
/// </summary>
/// <remarks>
/// Responsible for initializing core game components,
/// generating the level, and running the main game loop.
/// </remarks>
class Program
{
    /// <summary>
    /// Initializes the game state and starts the game loop.
    /// </summary>
    /// <remarks>
    /// Sets up the level, generator, player character, renderer,
    /// inventory and <see cref="InputHandler"/> before entering the
    /// main loop via <see cref="RunGameLoop"/>.
    /// </remarks>
    static async Task Main()
    {
        Console.CursorVisible = false;


        string configPath = Path.Combine(AppContext.BaseDirectory, "config.ini");
        GameConfig loadedConfig = IniConfigLoader.Load(configPath);
        Config.Initialize(loadedConfig);


        var level = new Level(Config.WindowWidth, Config.WindowHeight);
        var player = new Character.Player(
            new Position(Config.DefaultSpawnX, Config.DefaultSpawnY));

        DungeonThemeKind theme = DungeonThemePicker.Pick(Random.Shared);
        DungeonThemeProfile profile = DungeonThemeCatalog.Profile(theme);
        level.DungeonIntro = profile.IntroMessage;

        IDungeonStrategy strategy = DungeonStrategyCatalog.Resolve(theme);
        var generator = strategy.Create();

        await generator.GenerateAsync(level);

        level.GetTile(player.Pos.X, player.Pos.Y).IsOccupied = true;

        var renderer = new ConsoleRenderer();
        var inventory = new Inventory(player, 20);
        var inputHandler = new InputHandler();
        var memorySink = new InMemoryLogSink();
        var fileSink = new FileLogSink(Config.LogDirectory, Config.PlayerName, DateTime.Now);
        var logSink = new BufferedJournalLogSink(memorySink, fileSink, flushEveryNFrames: 10);
        GameLog.UseSink(logSink);
        GameLog.Write(new SessionStartedLogEvent(fileSink.FilePath));

        // register all input bindings with the renderer so the help menu
        // can be built automatically.  Add the F1 key manually as well.
        foreach (var binding in inputHandler.Bindings)
        {
            renderer.RegisterHelpEntry(binding.DisplayText, binding.Description);
        }

        Console.Clear();
        RunGameLoop(level, player, renderer, inventory, inputHandler, Config.TargetFPS, logSink);
        GameLog.Flush();
    }

    /// <summary>
    /// Executes the main game loop.
    /// </summary>
    /// <param name="level">The active level.</param>
    /// <param name="player">The player instance.</param>
    /// <param name="renderer">The console renderer.</param>
    /// <param name="inventory">The player's inventory.</param>
    /// <param name="inputHandler">Handler responsible for converting console
    /// keystrokes into game commands.</param>
    /// <param name="targetFPS">Target frames per second.</param>
    /// <param name="logSink">Buffered log sink that flushes entries by frame cadence.</param>
    /// <remarks>
    /// The loop repeatedly:
    /// <list type="number">
    /// <item><description>Renders the current game state.</description></item>
    /// <item><description>Processes player input using
    /// <see cref="InputHandler"/> and associated commands.</description></item>
    /// <item><description>Maintains frame timing.</description></item>
    /// </list>
    /// </remarks>
    private static void RunGameLoop(
        Level level,
        Player player,
        ConsoleRenderer renderer,
        Inventory inventory,
        InputHandler inputHandler,
        int targetFPS,
        BufferedJournalLogSink logSink)
    {
        var isRunning = true;

        // initial draw so the screen isn’t blank until the player presses a key
        renderer.Render(level, player, inventory);

        while (isRunning)
        {
            // handle input first so that pressing F1 immediately causes
            // the very next render call to show the help overlay.  When
            // help appears it will internally wait for a key, preventing
            // the key used to dismiss the popup from being interpreted as
            // a game command.
            var key = Console.ReadKey(true);
            InputResult result = player.Health <= 0 ? InputResult.Quit : inputHandler.HandleInput(key, level, player, inventory);
            if (result == InputResult.Quit)
            {
                isRunning = false;
            }
            else if (result == InputResult.Help)
            {
                renderer.ToggleHelpDisplay();
            }
            else if (result == InputResult.Journal)
            {
                renderer.ToggleJournalDisplay();
            }

            renderer.Render(level, player, inventory);
            logSink.AdvanceFrame();
            Thread.Sleep(Decimal.ToInt32(1000 / targetFPS));
        }
    }



}
