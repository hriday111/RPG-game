using System;
using RpgGame.Core;
using RpgGame.Generation;
using RpgGame.Rendering;
using RpgGame.Character;

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
    static void Main()
    {
        Console.CursorVisible = false;

        var level = new Level(Config.WindowWidth, Config.WindowHeight);
        IMapGenerator generator = new SimpleRoomGenerator();
        var player = new Character.Player(
            new Position(Config.DefaultSpawnX, Config.DefaultSpawnY));

        generator.Generate(level);

        var renderer = new ConsoleRenderer();
        var inventory = new Inventory(player, 20);
        Console.Clear();
        RunGameLoop(level, player, renderer,inventory, Config.TargetFPS);
    }

    /// <summary>
    /// Executes the main game loop.
    /// </summary>
    /// <param name="level">The active level.</param>
    /// <param name="player">The player instance.</param>
    /// <param name="renderer">The console renderer.</param>
    /// <param name="TargetFPS">Target frames per second.</param>
    /// <remarks>
    /// The loop repeatedly:
    /// <list type="number">
    /// <item><description>Renders the current game state.</description></item>
    /// <item><description>Processes player input.</description></item>
    /// <item><description>Maintains frame timing.</description></item>
    /// </list>
    /// </remarks>
    private static void RunGameLoop(
        Level level,
        Player player,
        ConsoleRenderer renderer,
        Inventory inventory,
        int TargetFPS)
    {
        var isRunning = true;

        while (isRunning)
        {
            renderer.Render(level, player, inventory);

            if (HandleInput(level, player, inventory) == -1)
                isRunning = false;

            Thread.Sleep(Decimal.ToInt32(1000 / TargetFPS));
        }
    }

    /// <summary>
    /// Handles user input and applies corresponding game actions.
    /// </summary>
    /// <param name="level">The current level.</param>
    /// <param name="player">The player instance.</param>
    /// <returns>
    /// -1 if the game should terminate; otherwise 0.
    /// </returns>
    /// <remarks>
    /// Supported controls:
    /// <list type="bullet">
    /// <item><description>W/A/S/D — Movement</description></item>
    /// <item><description>Q/E — Pickup or equip items</description></item>
    /// <item><description>Shift + Q/E — Drop equipped items</description></item>
    /// <item><description>Escape — Exit game</description></item>
    /// </list>
    /// 
    /// Movement validation is delegated to the <see cref="Level"/> class.
    /// Item behavior is handled polymorphically via <see cref="IItem"/>.
    /// </remarks>
    private static int HandleInput(Level level, Player player, Inventory inventory)
    {
        Position movement = new(0, 0);

        var key = Console.ReadKey(true);

        switch (key.Key)
        {
            case ConsoleKey.W:
                movement += Directions.Up;
                break;

            case ConsoleKey.S:
                movement += Directions.Down;
                break;

            case ConsoleKey.A:
                movement += Directions.Left;
                break;

            case ConsoleKey.D:
                movement += Directions.Right;
                break;

            
            case ConsoleKey.Q when key.Modifiers.HasFlag(ConsoleModifiers.Shift):
                player.DropLeft(level);
                break;

            case ConsoleKey.E when key.Modifiers.HasFlag(ConsoleModifiers.Shift):
                player.DropRight(level);
                break;
            case ConsoleKey.F:
                var item = level.GetTopItem(player.Pos);
                if (item != null)
                {
                    if(item.OnPickup(player, inventory)){level.TakeTopItem(player.Pos);}
                    
                }
                break;
            case ConsoleKey.E:
                inventory.TakeToRight();
                break;
            case ConsoleKey.Q:
                inventory.TakeToLeft();
                break;

            case ConsoleKey.Escape:
                return -1;
        }

        if (movement.X != 0 || movement.Y != 0)
        {
            var nPos = player.Pos + movement;

            level.GetTile(player.Pos.X, player.Pos.Y).IsOccupied = false;
            level.TryMoveCharacter(player, nPos);
            level.GetTile(nPos.X, nPos.Y).IsOccupied = true;
        }

        return 0;
    }
}