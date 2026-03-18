using RpgGame.Core;

namespace RpgGame.Generation.Procedures;

/// <summary>
/// Spawns weapons (swords and double swords) at random locations in the dungeon.
/// </summary>
/// <remarks>
/// This procedure places various sword-type weapons at random walkable positions throughout the level,
/// allowing the player to upgrade their combat capabilities.
/// </remarks>
public class AddWeaponsProcedure : IDungeonProcedure
{
    private readonly int SwordCount;
    private readonly int DoubleSwordCount;

    /// <summary>
    /// Initializes the weapons spawning procedure.
    /// </summary>
    /// <param name="SwordCount">The number of one-handed swords to spawn. Defaults to 2.</param>
    /// <param name="DoubleSwordCount">The number of two-handed swords to spawn. Defaults to 1.</param>
    public AddWeaponsProcedure(int SwordCount = 2, int DoubleSwordCount = 1)
    {
        this.SwordCount = 2;
        this.DoubleSwordCount = 1;
    }

    /// <summary>
    /// Applies the weapon spawning to the level.
    /// </summary>
    /// <param name="level">The level being generated.</param>
    /// <param name="context">Shared generation state.</param>
    /// <returns>A task that completes when all weapons have been placed.</returns>
    public async Task ApplyAsync(Level level, DungeonContext context)
    {
        await MapSpawnHelper.SpawnSwordAsync(level, SwordCount);
        await MapSpawnHelper.SpawnDoubleSwordAsync(level, DoubleSwordCount);
    }
}
