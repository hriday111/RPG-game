using System.Reflection.Metadata.Ecma335;

namespace RpgGame.Input;

using RpgGame.Character;
using RpgGame.Core;

/// <summary>
/// Picks up the item resting on the player's current tile and
/// places it into the inventory if possible.
/// </summary>
public class TakeToInventoryCommand : IInputCommand
{
    /// <inheritdoc/>
    public int Execute(Level level, Player player, Inventory inventory)
    {
        var item = level.GetTopItem(player.Pos);
        if (item != null)
        {
            if (item.OnPickup(player, inventory)) { level.TakeTopItem(player.Pos); }

        }
        return 1;
    }

}
