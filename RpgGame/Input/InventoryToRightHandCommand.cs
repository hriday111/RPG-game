namespace RpgGame.Input;

using RpgGame.Character;
using RpgGame.Core;
using RpgGame.Items;
using RpgGame.Logger;
/// <summary>
/// Transfers the currently selected inventory item to the player's right hand.
/// </summary>
public class InventoryToRightHandCommand : IInputCommand
{
    /// <inheritdoc/>
    public InputResult Execute(Level level, Player player, Inventory inventory)
    {
        IEquippable? selected = inventory.GetSelectedItem();
        if (selected == null)
            return InputResult.None;

        string equippedName = selected.Name;

        if (inventory.TakeToRight())
        {
            GameLog.Write(new ItemPickedUpLogEvent(equippedName));
            return InputResult.Ok;
        }

        return InputResult.None;
    }
}
