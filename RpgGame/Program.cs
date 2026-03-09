using System;
using System.Threading;
using System.Threading.Tasks;
using RpgGame.Core;
using RpgGame.Generation;
using RpgGame.Rendering;
using RpgGame.Character;
using RpgGame.Input;
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

        var level = new Level(Config.WindowWidth, Config.WindowHeight);
        IMapGenerator generator = new DungeonGenerator();
        var player = new Character.Player(
            new Position(Config.DefaultSpawnX, Config.DefaultSpawnY));

        await generator.GenerateAsync(level);

        var renderer = new ConsoleRenderer();
        var inventory = new Inventory(player, 20);
        var inputHandler = new InputHandler();
        Console.Clear();
        RunGameLoop(level, player, renderer,inventory, inputHandler, Config.TargetFPS);
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
    /// <param name="TargetFPS">Target frames per second.</param>
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
        int TargetFPS)
    {
        var isRunning = true;

        while (isRunning)
        {
            renderer.Render(level, player, inventory);
            var key = Console.ReadKey(true);
            if (inputHandler.HandleInput(key, level, player, inventory) == -1)
                isRunning = false;

            Thread.Sleep(Decimal.ToInt32(1000 / TargetFPS));
        }
    }


    
}