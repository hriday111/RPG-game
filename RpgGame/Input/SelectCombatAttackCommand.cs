using RpgGame.Character;
using RpgGame.Combat;
using RpgGame.Core;

namespace RpgGame.Input;

/// <summary>
/// Sets the player's next melee attack style (normal, stealth, or magical strike).
/// </summary>
public sealed class SelectCombatAttackCommand : IInputCommand
{
    private readonly ICombatAttack _attack;

    public SelectCombatAttackCommand(ICombatAttack attack)
    {
        _attack = attack;
    }

    /// <inheritdoc />
    public InputResult Execute(Level level, Player player, Inventory inventory)
    {
        player.SetSelectedCombatAttack(_attack);
        return InputResult.Ok;
    }
}
