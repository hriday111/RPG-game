namespace RpgGame.Input;

using RpgGame.Core;
using RpgGame.Character;

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