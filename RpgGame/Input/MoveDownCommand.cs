namespace RpgGame.Input;

using RpgGame.Character;
using RpgGame.Core;

/// <summary>
/// Moves the player one tile down (south) on the level.
/// </summary>
public class MoveDownCommand : IInputCommand
{
    /// <inheritdoc/>
    public InputResult Execute(Level level, Player player, Inventory inventory)
    {
        var nPos = player.Pos + Directions.Down;
        level.TryOrthogonalStepOrMeleeCombat(player, nPos);
        return InputResult.Ok;
    }
}
