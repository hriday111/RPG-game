namespace RpgGame.Input;

using RpgGame.Character;
using RpgGame.Core;

/// <summary>
/// Transfers the currently selected inventory item to the player's right hand.
/// </summary>
public class InventoryToRightHandCommand : IInputCommand
{
    /// <inheritdoc/>
    public int Execute(Level level, Player player, Inventory inventory)
    {
        inventory.TakeToRight();
        return 1;
    }
}
