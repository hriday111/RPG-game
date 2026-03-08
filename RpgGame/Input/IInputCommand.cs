namespace RpgGame.Input;

using RpgGame.Core;
using RpgGame.Character;

/// <summary>
/// Represents a user input command that manipulates the game
/// state (level, player, inventory) and returns an integer result.
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
    /// Typically <c>1</c> to indicate the game should continue,
    /// <c>-1</c> to signal exit, or other values for custom logic.
    /// </returns>
    int Execute(Level level, Player player, Inventory inventory);
}