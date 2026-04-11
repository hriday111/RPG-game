using System.Diagnostics.CodeAnalysis;
using RpgGame.Character;
using RpgGame.Combat;
using RpgGame.Items;
using RpgGame.Tiles;

namespace RpgGame.Core;

/// <summary>
/// Represents a single game level containing terrain tiles,
/// item placements, and movement validation logic.
/// </summary>
/// <remarks>
/// The Level class acts as the central authority for world state.
/// It manages:
/// - Tile grid storage
/// - Item placement on the ground
/// - Movement validation for characters
/// 
/// The class enforces boundary rules and ensures characters
/// cannot move into non-walkable or occupied tiles.
/// </remarks>
public class Level
{
    /// <summary>
    /// 2D array representing terrain tiles.
    /// </summary>
    private readonly Tile[,] tiles;

    /// <summary>
    /// Width of the level in tiles.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Height of the level in tiles.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Stores items located on specific positions in the level.
    /// Each position may contain multiple items.
    /// </summary>
    private readonly Dictionary<Position, List<IItem>> items = new();

    /// <summary>
    /// Non-player characters placed on this level (e.g. golems).
    /// </summary>
    private readonly List<Golem> golems = new();

    /// <summary>
    /// Gets the golems currently on the level.
    /// </summary>
    public IReadOnlyList<Golem> Golems => golems;

    /// <summary>
    /// Text describing the last orthogonal melee exchange, for the UI.
    /// </summary>
    public string? LastCombatMessage { get; private set; }

    /// <summary>
    /// Clears <see cref="LastCombatMessage"/> after a non-combat action.
    /// </summary>
    public void ClearCombatFeedback() => LastCombatMessage = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="Level"/> class.
    /// </summary>
    /// <param name="width">Width of the level grid.</param>
    /// <param name="height">Height of the level grid.</param>
    public Level(int width, int height)
    {
        Width = width;
        Height = height;
        tiles = new Tile[height, width];
    }

    #region Tile Management

    /// <summary>
    /// Retrieves the tile at the specified coordinates.
    /// </summary>
    /// <param name="x">Horizontal coordinate.</param>
    /// <param name="y">Vertical coordinate.</param>
    /// <returns>The tile located at the given position.</returns>
    public Tile GetTile(int x, int y)
    {
        return tiles[y, x];
    }

    /// <summary>
    /// Sets the tile at the specified coordinates.
    /// </summary>
    /// <param name="x">Horizontal coordinate.</param>
    /// <param name="y">Vertical coordinate.</param>
    /// <param name="tile">The tile to assign.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the tile parameter is null.
    /// </exception>
    public void SetTile(int x, int y, Tile tile)
    {
        tiles[y, x] = tile ?? throw new ArgumentNullException(nameof(tile));
    }

    #endregion

    #region Movement Logic

    /// <summary>
    /// Attempts to move a character to a new position if the move is valid.
    /// Also handles item interactions for players.
    /// </summary>
    /// <param name="character">The character attempting to move.</param>
    /// <param name="nPos">The target position.</param>
    public void TryMoveCharacter(Character.Character character, Position nPos)
    {
        if (IsMoveValid(character, nPos))
        {
            character.Move(nPos);


        }
    }

    /// <summary>
    /// Determines whether a character can move to the specified position.
    /// </summary>
    /// <param name="character">The character attempting to move.</param>
    /// <param name="nPos">The target position.</param>
    /// <returns>
    /// True if the move is within bounds, walkable, and unoccupied; otherwise false.
    /// </returns>
    public bool IsMoveValid(Character.Character character, Position nPos)
    {
        return IsInBounds(nPos)
            && GetTile(nPos.X, nPos.Y).IsWalkable
            && !GetTile(nPos.X, nPos.Y).IsOccupied;
    }

    /// <summary>
    /// Checks whether the specified position lies within the level boundaries.
    /// </summary>
    /// <param name="pos">The position to check.</param>
    /// <returns>True if the position is within bounds; otherwise false.</returns>
    public bool IsInBounds(Position pos)
    {
        return pos.X >= 0 && pos.X < Width &&
               pos.Y >= 0 && pos.Y < Height;
    }

    /// <summary>
    /// Places a golem on the level and marks its tile occupied for movement checks.
    /// </summary>
    public void AddGolem(Golem golem)
    {
        ArgumentNullException.ThrowIfNull(golem);
        var tile = GetTile(golem.Pos.X, golem.Pos.Y);
        if (!tile.IsWalkable)
            throw new InvalidOperationException("Cannot place a golem on a non-walkable tile.");

        tile.IsOccupied = true;
        golems.Add(golem);
    }

    /// <summary>
    /// Removes a golem from the level and frees its tile for movement.
    /// </summary>
    /// <returns>True if the golem was present and removed.</returns>
    public bool RemoveGolem(Golem golem)
    {
        ArgumentNullException.ThrowIfNull(golem);
        if (!golems.Remove(golem))
            return false;

        if (IsInBounds(golem.Pos))
            GetTile(golem.Pos.X, golem.Pos.Y).IsOccupied = false;

        return true;
    }

    /// <summary>
    /// When a golem occupies <paramref name="pos"/>, sets <paramref name="golem"/> and returns true.
    /// </summary>
    public bool TryGetGolemAt(Position pos, [NotNullWhen(true)] out Golem? golem)
    {
        foreach (var g in golems)
        {
            if (g.Pos == pos)
            {
                golem = g;
                return true;
            }
        }

        golem = null;
        return false;
    }

    /// <summary>
    /// One orthogonal step from the player (WASD): walk onto a free tile, or melee-attack
    /// a golem on the target tile. Diagonal offsets are rejected (distance must be 1 on the grid).
    /// </summary>
    /// <returns>True if a move or combat exchange was attempted on a valid target tile.</returns>
    public bool TryOrthogonalStepOrMeleeCombat(Player player, Position targetPos)
    {
        ArgumentNullException.ThrowIfNull(player);

        int manhattan = Math.Abs(targetPos.X - player.Pos.X) + Math.Abs(targetPos.Y - player.Pos.Y);
        if (manhattan != 1)
            return false;

        if (!IsInBounds(targetPos))
            return false;

        var tile = GetTile(targetPos.X, targetPos.Y);
        if (!tile.IsWalkable)
            return false;

        if (TryGetGolemAt(targetPos, out Golem? golemAtTarget))
        {
            CombatRoundResult result = CombatRound.Resolve(player, golemAtTarget);
            LastCombatMessage = DescribeCombatRound(result, golemAtTarget);
            if (golemAtTarget.IsDead)
                RemoveGolem(golemAtTarget);
            return true;
        }

        if (tile.IsOccupied)
            return false;

        ClearCombatFeedback();
        GetTile(player.Pos.X, player.Pos.Y).IsOccupied = false;
        player.Move(targetPos);
        tile.IsOccupied = true;
        return true;
    }

    private static string DescribeCombatRound(CombatRoundResult result, Golem golem)
    {
        string label = golem.EquippedWeapon.Name;
        if (result.EnemyDefeated)
            return $"You deal {result.DamageAppliedToEnemy} damage. {label} golem destroyed.";
        return $"You deal {result.DamageAppliedToEnemy} damage. {label} golem hits you for {result.DamageAppliedToPlayer}.";
    }

    #endregion

    #region Item Management

    /// <summary>
    /// Adds an item to the specified position in the level.
    /// </summary>
    /// <param name="pos">The position where the item should be placed.</param>
    /// <param name="item">The item to add.</param>
    public void AddItem(Position pos, IItem item)
    {
        if (!items.ContainsKey(pos))
        {
            items[pos] = new List<IItem>();
        }

        items[pos].Add(item);
    }

    /// <summary>
    /// Retrieves all items located at a given position.
    /// </summary>
    /// <param name="pos">The position to query.</param>
    /// <returns>
    /// A read-only list of items at the specified position.
    /// Returns an empty list if no items are present.
    /// </returns>
    public IReadOnlyList<IItem> GetItemsAt(Position pos)
    {
        if (items.TryGetValue(pos, out var list))
        {
            return list;
        }

        return Array.Empty<IItem>();
    }

    /// <summary>
    /// Retrieves the top item at a position without removing it.
    /// </summary>
    /// <param name="pos">The position to query.</param>
    /// <returns>
    /// The first item in the stack if present; otherwise null.
    /// </returns>
    public IItem? GetTopItem(Position pos)
    {
        if (items.TryGetValue(pos, out var list) && list.Count > 0)
            return list[0];

        return null;
    }

    /// <summary>
    /// Removes and returns the top item at a given position.
    /// </summary>
    /// <param name="pos">The position from which to take the item.</param>
    /// <returns>
    /// The removed item if present; otherwise null.
    /// </returns>
    public IItem? TakeTopItem(Position pos)
    {
        if (!items.TryGetValue(pos, out var list) || list.Count == 0)
            return null;

        var item = list[0];
        list.RemoveAt(0);

        if (list.Count == 0)
            items.Remove(pos);

        return item;
    }

    /// <summary>
    /// Returns the total number of items at a given position.
    /// </summary>
    /// <param name="pos">The position to query.</param>
    /// <returns>The number of items at that position.</returns>
    public int TotalItemsAt(Position pos)
    {
        if (!items.TryGetValue(pos, out var list) || list.Count == 0)
            return 0;

        return list.Count;
    }

    /// <summary>
    /// Determines whether any items exist at a given position.
    /// </summary>
    /// <param name="pos">The position to check.</param>
    /// <returns>True if items exist; otherwise false.</returns>
    public bool HasItems(Position pos)
    {
        return items.ContainsKey(pos);
    }

    #endregion
}
