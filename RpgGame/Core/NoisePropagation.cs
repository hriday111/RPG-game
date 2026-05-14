using System.Collections.Generic;
using RpgGame.Character;

namespace RpgGame.Core;

/// <summary>
/// Observer participant: an enemy that may react when weapon-handling noise reaches its tile.
/// </summary>
public interface INoiseListener
{
    Position ListenerTile { get; }

    /// <summary>
    /// Invoked when a weapon pickup/equip noise reaches this listener along walkable tiles only.
    /// </summary>
    void OnWeaponPickupNoise(Position soundSource, int graphDistanceSteps);
}

/// <summary>
/// Orthogonal BFS on walkable tiles (walls block; occupancy ignored so sound can cross open corridors/rooms).
/// </summary>
internal static class WalkableNoiseReach
{
    public static Dictionary<Position, int> DistancesWithin(Level level, Position start, int maxSteps)
    {
        var distances = new Dictionary<Position, int>();
        if (maxSteps <= 0)
            return distances;

        var queue = new Queue<(Position Pos, int Dist)>();
        distances[start] = 0;
        queue.Enqueue((start, 0));

        while (queue.Count > 0)
        {
            (Position p, int d) = queue.Dequeue();
            if (d >= maxSteps)
                continue;

            TryEnqueueNeighbor(level, distances, queue, d, new Position(p.X, p.Y - 1), maxSteps);
            TryEnqueueNeighbor(level, distances, queue, d, new Position(p.X, p.Y + 1), maxSteps);
            TryEnqueueNeighbor(level, distances, queue, d, new Position(p.X - 1, p.Y), maxSteps);
            TryEnqueueNeighbor(level, distances, queue, d, new Position(p.X + 1, p.Y), maxSteps);
        }

        return distances;
    }

    private static void TryEnqueueNeighbor(
        Level level,
        Dictionary<Position, int> distances,
        Queue<(Position Pos, int Dist)> queue,
        int fromDist,
        Position neighbor,
        int maxSteps)
    {
        if (!level.IsInBounds(neighbor))
            return;

        if (!level.GetTile(neighbor.X, neighbor.Y).IsWalkable)
            return;

        int next = fromDist + 1;
        if (next > maxSteps)
            return;

        if (distances.ContainsKey(neighbor))
            return;

        distances[neighbor] = next;
        queue.Enqueue((neighbor, next));
    }
}
