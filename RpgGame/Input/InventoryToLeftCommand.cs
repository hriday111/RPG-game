namespace RpgGame.Input;

using RpgGame.Character;
using RpgGame.Core;

/// <summary>
/// Transfers the currently selected inventory item to the player's left hand.
/// </summary>
public class InventoryToLeftHandCommand : IInputCommand
{
    /// <inheritdoc/>
    public int Execute(Level level, Player player, Inventory inventory)
    {
        inventory.TakeToLeft();
        return 1;
    }
}
