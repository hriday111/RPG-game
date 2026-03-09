using RpgGame.Core;
using RpgGame.Character;
namespace RpgGame.Input;
/// <summary>
/// Maps console key combinations to their corresponding
/// <see cref="IInputCommand"/> implementations and
/// invokes them when input is received.
/// </summary>
public class InputHandler
{
    private readonly Dictionary<(ConsoleKey, ConsoleModifiers), IInputCommand> commands;

    /// <summary>
    /// Initializes the input command map with default bindings.
    /// </summary>
    public InputHandler()
    {
        commands = new()
        {
            {(ConsoleKey.W, 0), new MoveUpCommand()},
            {(ConsoleKey.S, 0), new MoveDownCommand()},
            {(ConsoleKey.A, 0), new MoveLeftCommand()},
            {(ConsoleKey.D, 0), new MoveRightCommand()},
            {(ConsoleKey.F, 0), new TakeToInventoryCommand()},
            {(ConsoleKey.Q, 0), new InventoryToLeftHandCommand()},
            {(ConsoleKey.E, 0), new InventoryToRightHandCommand()},
            {(ConsoleKey.Escape, 0), new QuitGameCommand()},
            {(ConsoleKey.Q, ConsoleModifiers.Shift), new DropLeftCommand()},
            {(ConsoleKey.E, ConsoleModifiers.Shift), new DropRightCommand()}
        };
    }

    /// <summary>
    /// Processes a <see cref="ConsoleKeyInfo"/> entry and executes
    /// the associated command if one exists.
    /// </summary>
    /// <param name="key">The key info read from the console.</param>
    /// <param name="level">The current level.</param>
    /// <param name="player">The player character.</param>
    /// <param name="inventory">The player's inventory.</param>
    /// <returns>
    /// Command execution result (see individual commands). Returns
    /// <c>-1</c> when esc is pressed.
    /// </returns>
    public int HandleInput(ConsoleKeyInfo key, Level level, Player player, Inventory inventory)
    {
        var combo = (key.Key, key.Modifiers);

        if (commands.TryGetValue(combo, out var command))
        {
            return command.Execute(level, player, inventory);
        }
        return 0;
    }
}