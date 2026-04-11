using RpgGame.Core;
using RpgGame.Generation;

namespace RpgGame.Generation.Procedures;


public class AddEnemiesProcedure : IDungeonProcedure
{
    private readonly int GolemCount = 1;

    public AddEnemiesProcedure(int GolemCount = 1)
    {
        this.GolemCount = GolemCount;
    }

    public async Task ApplyAsync(Level level, DungeonContext context)
    {
        await MapSpawnHelper.SpawnGolemAsync(level, GolemCount);
    }
}
