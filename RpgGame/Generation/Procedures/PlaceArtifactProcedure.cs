using RpgGame.Core;
using RpgGame.Items;

namespace RpgGame.Generation.Procedures;

/// <summary>
/// Places exactly one thematic artifact on a random walkable tile (factory supplied by the active theme profile).
/// </summary>
public sealed class PlaceArtifactProcedure : IDungeonProcedure
{
    private readonly Func<IItem> createArtifact;

    public PlaceArtifactProcedure(Func<IItem> createArtifact)
    {
        this.createArtifact = createArtifact ?? throw new ArgumentNullException(nameof(createArtifact));
    }

    public Task ApplyAsync(Level level, DungeonContext context) =>
        MapSpawnHelper.SpawnSingleItemAsync(level, createArtifact());
}
