namespace RpgGame.Input;

using RpgGame.Character;
using RpgGame.Core;
using RpgGame.Logger;

/// <summary>
/// Picks up the item resting on the player's current tile and
/// places it into the inventory if possible.
/// </summary>
public class TakeToInventoryCommand : IInputCommand
{
    /// <inheritdoc/>
    public InputResult Execute(Level level, Player player, Inventory inventory)
    {
        var item = level.GetTopItem(player.Pos);
        if (item != null)
        {
            if (item.OnPickup(player, inventory))
            {
                GameLog.Write(new ItemPickedUpLogEvent(item.Name));
                level.TakeTopItem(player.Pos);
            }

        }
        return InputResult.Ok;
    }

}
