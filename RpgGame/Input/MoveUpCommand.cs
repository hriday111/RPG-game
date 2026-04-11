namespace RpgGame.Input;

using RpgGame.Character;
using RpgGame.Core;

/// <summary>
/// Moves the player one tile up (north) on the level.
/// </summary>
public class MoveUpCommand : IInputCommand
{
    /// <inheritdoc/>
    public InputResult Execute(Level level, Player player, Inventory inventory)
    {
        var nPos = player.Pos + Directions.Up;
        level.TryOrthogonalStepOrMeleeCombat(player, nPos);
        return InputResult.Ok;
    }
}
