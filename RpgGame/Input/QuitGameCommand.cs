namespace RpgGame.Input;

using RpgGame.Character;
using RpgGame.Core;

/// <summary>
/// Command that causes the game loop to exit.
/// </summary>
public class QuitGameCommand : IInputCommand
{
    /// <inheritdoc/>
    public int Execute(Level level, Player player, Inventory inventory)
    {
        return -1;
    }
}
