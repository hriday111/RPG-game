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
    // keep a parallel list of descriptors so other systems (renderer/help) can query
    private readonly List<ControlDescriptor> controlDescriptors = new();

    /// <summary>
    /// Initializes the input command map with default bindings.
    /// </summary>
    public InputHandler()
    {
        commands = new();

        // helper that registers both the command and a description
        void Register(ConsoleKey key, ConsoleModifiers mods, IInputCommand cmd, string description)
        {
            commands[(key, mods)] = cmd;
            controlDescriptors.Add(new ControlDescriptor(key, mods, description));
        }

        Register(ConsoleKey.W, 0, new MoveUpCommand(), "Move up");
        Register(ConsoleKey.S, 0, new MoveDownCommand(), "Move down");
        Register(ConsoleKey.A, 0, new MoveLeftCommand(), "Move left");
        Register(ConsoleKey.D, 0, new MoveRightCommand(), "Move right");
        Register(ConsoleKey.F, 0, new TakeToInventoryCommand(), "Pick up / take item");
        Register(ConsoleKey.Q, 0, new InventoryToLeftHandCommand(), "Equip to left hand");
        Register(ConsoleKey.E, 0, new InventoryToRightHandCommand(), "Equip to right hand");
        Register(ConsoleKey.D1, 0, new SelectInventoryCommand(0), "Select inventory slot 1");
        Register(ConsoleKey.D2, 0, new SelectInventoryCommand(1), "Select inventory slot 2");
        Register(ConsoleKey.D3, 0, new SelectInventoryCommand(2), "Select inventory slot 3");
        Register(ConsoleKey.D4, 0, new SelectInventoryCommand(3), "Select inventory slot 4");
        Register(ConsoleKey.D5, 0, new SelectInventoryCommand(4), "Select inventory slot 5");
        Register(ConsoleKey.D6, 0, new SelectInventoryCommand(5), "Select inventory slot 6");
        Register(ConsoleKey.D7, 0, new SelectInventoryCommand(6), "Select inventory slot 7");
        Register(ConsoleKey.D8, 0, new SelectInventoryCommand(7), "Select inventory slot 8");
        Register(ConsoleKey.D9, 0, new SelectInventoryCommand(8), "Select inventory slot 9");
        Register(ConsoleKey.D0, 0, new SelectInventoryCommand(9), "Select inventory slot 10");
        Register(ConsoleKey.Escape, 0, new QuitGameCommand(), "Quit game");
        Register(ConsoleKey.Q, ConsoleModifiers.Shift, new DropLeftCommand(), "Drop left-hand item");
        Register(ConsoleKey.E, ConsoleModifiers.Shift, new DropRightCommand(), "Drop right-hand item");
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
    /// <summary>
    /// A read-only collection of all registered bindings along with descriptions.
    /// Useful for building a help menu.
    /// </summary>
    public IReadOnlyList<ControlDescriptor> Bindings => controlDescriptors;

    public int HandleInput(ConsoleKeyInfo key, Level level, Player player, Inventory inventory)
    {
        // show help when F1 is pressed; caller will toggle the help flag
        if (key.Key == ConsoleKey.F1)
            return 2;

        var combo = (key.Key, key.Modifiers);

        if (commands.TryGetValue(combo, out var command))
        {
            return command.Execute(level, player, inventory);
        }
        return 0;
    }
}