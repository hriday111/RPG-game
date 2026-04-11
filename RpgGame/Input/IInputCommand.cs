namespace RpgGame.Input;

using RpgGame.Character;
using RpgGame.Core;

/// <summary>
/// Represents a user input command that manipulates the game
/// state (level, player, inventory) and returns a <see cref="InputResult"/>.
/// </summary>
public interface IInputCommand
{
    /// <summary>
    /// Executes the command using provided game objects.
    /// </summary>
    /// <param name="level">The current level.</param>
    /// <param name="player">The player character.</param>
    /// <param name="inventory">The player's inventory.</param>
    /// <returns>
    /// Usually <see cref="InputResult.Ok"/> to continue,
    /// <see cref="InputResult.Quit"/> to exit, or <see cref="InputResult.Help"/> to toggle help.
    /// </returns>
    InputResult Execute(Level level, Player player, Inventory inventory);
}
