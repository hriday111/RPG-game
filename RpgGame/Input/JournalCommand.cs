namespace RpgGame.Input;

using RpgGame.Character;
using RpgGame.Core;

/// <summary>
/// Command that opens the journal overlay.
/// </summary>
public class JournalCommand : IInputCommand
{
    /// <inheritdoc/>
    public InputResult Execute(Level level, Player player, Inventory inventory)
    {
        return InputResult.Journal;
    }
}
