namespace RpgGame.Input;

using RpgGame.Character;
using RpgGame.Core;
using RpgGame.Logger;
/// <summary>
/// Transfers the currently selected inventory item to the player's right hand.
/// </summary>
public class InventoryToRightHandCommand : IInputCommand
{
    /// <inheritdoc/>
    public InputResult Execute(Level level, Player player, Inventory inventory)
    {
        
        if(inventory.TakeToRight())
        {
        GameLog.Write(new ItemPickedUpLogEvent(inventory.GetSelectedItem().Name));
        return InputResult.Ok;
        }
        else
        {
            return InputResult.None;
        }
    }
}
