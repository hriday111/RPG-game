namespace RpgGame.Input;

using RpgGame.Core;
using RpgGame.Character;

/// <summary>
/// Instructs the player to drop the item in their right hand onto the level.
/// </summary>
public class DropRightCommand : IInputCommand
{
    /// <inheritdoc/>
    public int Execute(Level level, Player player, Inventory inventory)
    {
        player.DropRight(level);
        return 1;
    }
}