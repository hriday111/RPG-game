namespace RpgGame.Input;

using RpgGame.Character;
using RpgGame.Core;

/// <summary>
/// Instructs the player to drop the item in their right hand onto the level.
/// </summary>
public class DropRightCommand : IInputCommand
{
    /// <inheritdoc/>
    public InputResult Execute(Level level, Player player, Inventory inventory)
    {
        player.DropRight(level);
        return InputResult.Ok;
    }
}
