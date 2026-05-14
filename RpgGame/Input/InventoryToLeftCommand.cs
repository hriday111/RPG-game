namespace RpgGame.Input;

using RpgGame.Character;
using RpgGame.Core;
using RpgGame.Items;

/// <summary>
/// Transfers the currently selected inventory item to the player's left hand.
/// </summary>
public class InventoryToLeftHandCommand : IInputCommand
{
    /// <inheritdoc/>
    public InputResult Execute(Level level, Player player, Inventory inventory)
    {
        IEquippable? selected = inventory.GetSelectedItem();
        if (selected == null)
            return InputResult.None;

        if (!inventory.TakeToLeft())
            return InputResult.None;

        selected.EmitInventoryWeaponNoise(level, player.Pos);
        return InputResult.Ok;
    }
}
