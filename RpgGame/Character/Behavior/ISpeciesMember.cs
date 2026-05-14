namespace RpgGame.Character.Behavior;

/// <summary>
/// Enemies that share collective reactions within a <see cref="SpeciesKind"/>.
/// </summary>
public interface ISpeciesMember
{
    /// <summary>Which species roster this instance belongs to for registration and death broadcasts.</summary>
    SpeciesKind Kind { get; }

    void OnSpeciesMemberDeath();
}
