namespace RpgGame.Input;

using RpgGame.Core;
using RpgGame.Character;

/// <summary>
/// Instructs the player to drop the item in their left hand onto the level.
/// </summary>
public class DropLeftCommand : IInputCommand
{
    /// <inheritdoc/>
    public int Execute(Level level, Player player, Inventory inventory)
    {
        player.DropLeft(level);
        return 1;
    }
}