using RpgGame.Items;

namespace RpgGame.Character;

/// <summary>
/// Represents a Golem character in the game. Golems are typically non-player characters (NPCs) that may serve as enemies.
/// </summary>
/// <remarks> This implementation is just a basic skeleton. Nothing is added yet, and Golems aren't initialized anywhere in the game. </remarks>
public class Golem : Character
{
    public override char Symbol => 'O';
    IEquippable? EquippedItem { get; set; }
    public Golem(Position startPosition)
        : base(startPosition)
    {
        EquippedItem = new Sword();
    }


}
