using RpgGame.Character;
using RpgGame.Core;

namespace RpgGame.Input;

/// <summary>
/// Selects a specific inventory slot.
/// </summary>
public class SelectInventoryCommand : IInputCommand
{
    private readonly int _slotIndex;

    public SelectInventoryCommand(int slotIndex)
    {
        _slotIndex = slotIndex;
    }

    public InputResult Execute(Level level, Player player, Inventory inventory)
    {
        inventory.SelectedIndex = _slotIndex;
        return InputResult.Ok;
    }
}
