using System.Diagnostics.CodeAnalysis;
using RpgGame.Character;
using RpgGame.Character.Behavior;
using RpgGame.Combat;
using RpgGame.Items;
using RpgGame.Logger;
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
    /// Mage enemies on this level.
    /// </summary>
    private readonly List<Mage> mages = new();
    private readonly List<Goblin> goblins = new();
    private readonly List<Skeleton> skeletons = new();
    private readonly SpeciesGroup<Goblin> goblinSpecies = new();
    private readonly SpeciesGroup<Skeleton> skeletonSpecies = new();
    private static readonly Random random = new();
    public IReadOnlyList<Goblin> Goblins => goblins;
    public IReadOnlyList<Skeleton> Skeletons => skeletons;
    /// <summary>
    /// Gets the golems currently on the level.
    /// </summary>
    public IReadOnlyList<Golem> Golems => golems;

    /// <summary>
    /// Gets the mages currently on the level.
    /// </summary>
    public IReadOnlyList<Mage> Mages => mages;

    /// <summary>
    /// Text describing the last orthogonal melee exchange, for the UI.
    /// </summary>
    public string? LastCombatMessage { get; private set; }

    /// <summary>
    /// One-line thematic intro for this dungeon (from the active <c>DungeonThemeProfile</c>).
    /// </summary>
    public string? DungeonIntro { get; set; }

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
    public void AddGoblin(Goblin goblin)
    {
        ArgumentNullException.ThrowIfNull(goblin);
        var tile = GetTile(goblin.Pos.X, goblin.Pos.Y);
        if (!tile.IsWalkable)
            throw new InvalidOperationException("Cannot place a goblin on a non-walkable tile.");

        tile.IsOccupied = true;
        goblins.Add(goblin);
        goblinSpecies.Register(goblin);
    }

    public bool RemoveGoblin(Goblin goblin)
    {
        ArgumentNullException.ThrowIfNull(goblin);
        if (!goblins.Remove(goblin))
            return false;

        goblinSpecies.Unregister(goblin);

        if (IsInBounds(goblin.Pos))
            GetTile(goblin.Pos.X, goblin.Pos.Y).IsOccupied = false;

        return true;
    }

    public bool TryGetGoblinAt(Position pos, out Goblin? goblin)
    {
        foreach (var g in goblins)
        {
            if (g.Pos == pos)
            {
                goblin = g;
                return true;
            }
        }

        goblin = null;
        return false;
    }

    public void AddSkeleton(Skeleton skeleton)
    {
        ArgumentNullException.ThrowIfNull(skeleton);
        var tile = GetTile(skeleton.Pos.X, skeleton.Pos.Y);
        if (!tile.IsWalkable)
            throw new InvalidOperationException("Cannot place a skeleton on a non-walkable tile.");

        tile.IsOccupied = true;
        skeletons.Add(skeleton);
        skeletonSpecies.Register(skeleton);
    }

    public bool RemoveSkeleton(Skeleton skeleton)
    {
        ArgumentNullException.ThrowIfNull(skeleton);
        if (!skeletons.Remove(skeleton))
            return false;

        skeletonSpecies.Unregister(skeleton);

        if (IsInBounds(skeleton.Pos))
            GetTile(skeleton.Pos.X, skeleton.Pos.Y).IsOccupied = false;

        return true;
    }

    public bool TryGetSkeletonAt(Position pos, out Skeleton? skeleton)
    {
        foreach (var s in skeletons)
        {
            if (s.Pos == pos)
            {
                skeleton = s;
                return true;
            }
        }

        skeleton = null;
        return false;
    }
    /// <summary>
    /// Places a golem on the level and marks its tile occupied for movement checks.
    /// </summary>
    ///
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
    /// Places a mage on the level and marks its tile occupied for movement checks.
    /// </summary>
    public void AddMage(Mage mage)
    {
        ArgumentNullException.ThrowIfNull(mage);
        var tile = GetTile(mage.Pos.X, mage.Pos.Y);
        if (!tile.IsWalkable)
            throw new InvalidOperationException("Cannot place a mage on a non-walkable tile.");

        tile.IsOccupied = true;
        mages.Add(mage);
    }

    /// <summary>
    /// Removes a mage from the level and frees its tile for movement.
    /// </summary>
    /// <returns>True if the mage was present and removed.</returns>
    public bool RemoveMage(Mage mage)
    {
        ArgumentNullException.ThrowIfNull(mage);
        if (!mages.Remove(mage))
            return false;

        if (IsInBounds(mage.Pos))
            GetTile(mage.Pos.X, mage.Pos.Y).IsOccupied = false;

        return true;
    }

    /// <summary>
    /// When a mage occupies <paramref name="pos"/>, sets <paramref name="mage"/> and returns true.
    /// </summary>
    public bool TryGetMageAt(Position pos, [NotNullWhen(true)] out Mage? mage)
    {
        foreach (var m in mages)
        {
            if (m.Pos == pos)
            {
                mage = m;
                return true;
            }
        }

        mage = null;
        return false;
    }

    /// <summary>
    /// One orthogonal step from the player (WASD): walk onto a free tile, or melee-attack
    /// a golem or mage on the target tile. Diagonal offsets are rejected (distance must be 1 on the grid).
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
            GameLog.Write(new AttackDealtLogEvent(result.DamageAppliedToEnemy, $"{golemAtTarget.EquippedWeapon.Name} golem"));
            if (golemAtTarget.IsDead)
            {
                GameLog.Write(new EnemyDefeatedLogEvent($"{golemAtTarget.EquippedWeapon.Name} golem"));
                RemoveGolem(golemAtTarget);
            }
            return true;
        }

        if (TryGetMageAt(targetPos, out Mage? mageAtTarget))
        {
            CombatRoundResult result = CombatRound.Resolve(player, mageAtTarget);
            LastCombatMessage = DescribeCombatRound(result, mageAtTarget);
            GameLog.Write(new AttackDealtLogEvent(result.DamageAppliedToEnemy, $"{mageAtTarget.EquippedWeapon.Name} mage"));
            if (mageAtTarget.IsDead)
            {
                GameLog.Write(new EnemyDefeatedLogEvent($"{mageAtTarget.EquippedWeapon.Name} mage"));
                RemoveMage(mageAtTarget);
            }
            return true;
        }
        if (TryGetGoblinAt(targetPos, out Goblin? goblinAtTarget))
        {
            CombatRoundResult result = CombatRound.Resolve(player, goblinAtTarget);
            LastCombatMessage = DescribeCombatRound(result, goblinAtTarget);
            GameLog.Write(new AttackDealtLogEvent(result.DamageAppliedToEnemy, $"{goblinAtTarget.EquippedWeapon.Name} goblin"));

            if (goblinAtTarget.IsDead)
            {
                // Broadcast to species BEFORE removing from board.
                goblinSpecies.NotifyMemberDeath(goblinAtTarget);
                GameLog.Write(new EnemyDefeatedLogEvent($"{goblinAtTarget.EquippedWeapon.Name} goblin"));
                RemoveGoblin(goblinAtTarget);
            }

            return true;
        }

        if (TryGetSkeletonAt(targetPos, out Skeleton? skeletonAtTarget))
        {
            CombatRoundResult result = CombatRound.Resolve(player, skeletonAtTarget);
            LastCombatMessage = DescribeCombatRound(result, skeletonAtTarget);
            GameLog.Write(new AttackDealtLogEvent(result.DamageAppliedToEnemy, $"{skeletonAtTarget.EquippedWeapon.Name} skeleton"));

            if (skeletonAtTarget.IsDead)
            {
                skeletonSpecies.NotifyMemberDeath(skeletonAtTarget);
                GameLog.Write(new EnemyDefeatedLogEvent($"{skeletonAtTarget.EquippedWeapon.Name} skeleton"));
                RemoveSkeleton(skeletonAtTarget);
            }

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

    private static string DescribeCombatRound(CombatRoundResult result, Mage mage)
    {
        string label = mage.EquippedWeapon.Name;
        if (result.EnemyDefeated)
            return $"You deal {result.DamageAppliedToEnemy} damage. {label} mage destroyed.";
        return $"You deal {result.DamageAppliedToEnemy} damage. {label} mage hits you for {result.DamageAppliedToPlayer}.";
    }
    private static string DescribeCombatRound(CombatRoundResult result, Goblin goblin)
    {
        string label = goblin.EquippedWeapon.Name;
        if (result.EnemyDefeated)
            return $"You deal {result.DamageAppliedToEnemy} damage. {label} goblin destroyed.";
        return $"You deal {result.DamageAppliedToEnemy} damage. {label} goblin hits you for {result.DamageAppliedToPlayer}.";
    }

    private static string DescribeCombatRound(CombatRoundResult result, Skeleton skeleton)
    {
        string label = skeleton.EquippedWeapon.Name;
        if (result.EnemyDefeated)
            return $"You deal {result.DamageAppliedToEnemy} damage. {label} skeleton destroyed.";
        return $"You deal {result.DamageAppliedToEnemy} damage. {label} skeleton hits you for {result.DamageAppliedToPlayer}.";
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
    /// <summary>
    /// After each successful player command, all enemies take one random orthogonal step
    /// onto an unoccupied walkable tile (same turn cadence as the player).
    /// </summary>
    public void AdvanceReactiveEnemiesTurn()
    {
        AdvanceGroup(golems);
        AdvanceGroup(mages);
        AdvanceGroup(goblins);
        AdvanceGroup(skeletons);
    }

    private void AdvanceGroup<TEnemy>(IReadOnlyList<TEnemy> group) where TEnemy : Character.Character
    {
        // Snapshot because enemy positions mutate during the step.
        var snapshot = new List<TEnemy>(group);
        foreach (var enemy in snapshot)
            TryMoveEnemyRandomly(enemy);
    }

    private void TryMoveEnemyRandomly(Character.Character enemy)
    {
        var candidates = new List<Position>(4);

        AddCandidate(enemy.Pos + Directions.Up, candidates);
        AddCandidate(enemy.Pos + Directions.Down, candidates);
        AddCandidate(enemy.Pos + Directions.Left, candidates);
        AddCandidate(enemy.Pos + Directions.Right, candidates);

        if (candidates.Count == 0)
            return;

        Position target = candidates[random.Next(candidates.Count)];
        GetTile(enemy.Pos.X, enemy.Pos.Y).IsOccupied = false;
        enemy.Move(target);
        GetTile(target.X, target.Y).IsOccupied = true;
    }

    private void AddCandidate(Position candidate, List<Position> candidates)
    {
        if (!IsInBounds(candidate))
            return;

        var tile = GetTile(candidate.X, candidate.Y);
        if (!tile.IsWalkable || tile.IsOccupied)
            return;

        candidates.Add(candidate);
    }
}
