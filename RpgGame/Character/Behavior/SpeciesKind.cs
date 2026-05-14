namespace RpgGame.Character.Behavior;

/// <summary>
/// Stable key for grouping same-species observers. Add a value per reactive species;
/// <see cref="Core.Level"/> stores one member list per kind, like items keyed by position.
/// </summary>
public enum SpeciesKind
{
    Goblin,
    Skeleton,
}
