namespace RpgGame.Input;

using RpgGame.Character;
using RpgGame.Core;

/// <summary>
/// Moves the player one tile left (west) on the level.
/// </summary>
public class MoveLeftCommand : IInputCommand
{
    /// <inheritdoc/>
    public InputResult Execute(Level level, Player player, Inventory inventory)
    {
        var nPos = player.Pos + Directions.Left;
        level.TryOrthogonalStepOrMeleeCombat(player, nPos);
        return InputResult.Ok;
    }
}
