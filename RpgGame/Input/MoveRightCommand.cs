namespace RpgGame.Input;

using RpgGame.Character;
using RpgGame.Core;

/// <summary>
/// Moves the player one tile right (east) on the level.
/// </summary>
public class MoveRightCommand : IInputCommand
{
    /// <inheritdoc/>
    public int Execute(Level level, Player player, Inventory inventory)
    {
        var nPos = player.Pos + Directions.Right;

        level.GetTile(player.Pos.X, player.Pos.Y).IsOccupied = false;
        level.TryMoveCharacter(player, nPos);
        level.GetTile(nPos.X, nPos.Y).IsOccupied = true;
        return 1;
    }
}
