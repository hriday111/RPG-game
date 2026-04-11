using RpgGame.Character;
using RpgGame.Items;

namespace RpgGame.Combat;

/// <summary>
/// Weapon classification for combat: dispatches to the active <see cref="ICombatAttack"/>
/// without type tags (each concrete weapon composes one category instance).
/// </summary>
public interface IWeaponCategory
{
    void DispatchCombat(ICombatAttack attack, IWeapon weapon, Player player, ICombatContribution contribution);
}
