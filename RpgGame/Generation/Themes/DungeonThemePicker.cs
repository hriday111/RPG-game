namespace RpgGame.Generation.Themes;

public static class DungeonThemePicker
{
    private static readonly DungeonThemeKind[] OrderedKinds=
    [
        DungeonThemeKind.Basic,
        DungeonThemeKind.Treasure,
        DungeonThemeKind.Library,
        DungeonThemeKind.Healing,
    ];

    private static ReadOnlySpan<int> Weights=>[40,40,10,10];
    public static DungeonThemeKind Pick(Random random)
    {
        int roll = random.Next(100);
        int cumulative = 0;
        ReadOnlySpan<int> w = Weights;
        for (int i = 0; i < w.Length; i++)
        {
            cumulative += w[i];
            if (roll < cumulative)
                return OrderedKinds[i];
        }
        return OrderedKinds[^1];
    }
}