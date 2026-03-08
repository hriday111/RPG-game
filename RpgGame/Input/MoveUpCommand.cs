namespace RpgGame.Input;

using RpgGame.Core;
using RpgGame.Character;

/// <summary>
/// Moves the player one tile up (north) on the level.
/// </summary>
public class MoveUpCommand : IInputCommand
{
    /// <inheritdoc/>
    public int Execute(Level level, Player player, Inventory inventory)
    {
        var nPos = player.Pos + Directions.Up;

        level.GetTile(player.Pos.X, player.Pos.Y).IsOccupied = false;
        level.TryMoveCharacter(player, nPos);
        level.GetTile(nPos.X, nPos.Y).IsOccupied = true;
        return 1;
    }
}