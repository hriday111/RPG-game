using RpgGame.Core;
using RpgGame.Items;

namespace RpgGame.Generation.Procedures;

/// <summary>
/// Spawns treasure items (coins and gold) at random locations in the dungeon.
/// </summary>
/// <remarks>
/// This procedure places coins and gold piles at random walkable positions throughout the level,
/// giving the player opportunities to collect valuable items.
/// </remarks>
public class AddItemsProcedure : IDungeonProcedure
{
    private readonly int CoinCount;
    private readonly int GoldCount;

    /// <summary>
    /// Initializes the items spawning procedure.
    /// </summary>
    /// <param name="CoinCount">The number of coins to spawn. Defaults to 5.</param>
    /// <param name="GoldCount">The number of gold piles to spawn. Defaults to 2.</param>
    public AddItemsProcedure(int CoinCount= 5, int GoldCount= 2)
    {
        this.CoinCount = CoinCount;
        this.GoldCount = GoldCount;
    }

    /// <summary>
    /// Applies the item spawning to the level.
    /// </summary>
    /// <param name="level">The level being generated.</param>
    /// <param name="context">Shared generation state.</param>
    /// <returns>A task that completes when all items have been placed.</returns>
    public async Task ApplyAsync(Level level,  DungeonContext context)
    {
        await MapSpawnHelper.SpawnCoinsAsync(level, CoinCount);
        await MapSpawnHelper.SpawnGoldAsync(level, GoldCount);
    }
}