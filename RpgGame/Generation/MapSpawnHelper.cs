using System;
using System.Threading.Tasks;
using RpgGame.Character;
using RpgGame.Core;
using RpgGame.Items;
using RpgGame.Items.Modifiers;

namespace RpgGame.Generation
{
    /// <summary>
    /// Provides common item-spawning routines that can be shared among
    /// multiple <see cref="IMapGenerator"/> implementations.
    /// </summary>
    /// <remarks>
    /// The helpers choose random walkable positions on the map and place
    /// the requested items there.  By centralizing the logic here, generators
    /// don't have to duplicate the same loops and checks over and over.
    /// </remarks>
    public static class MapSpawnHelper
    {
        /// <summary>
        /// Random number generator used internally by the helper methods.
        /// </summary>
        private static readonly Random random = new();
        /// <summary>
        /// Picks a sword or double sword and applies the same random modifiers as floor loot weapons.
        /// </summary>
        private static IWeapon CreateRandomGolemWeapon()
        {
            Weapon core = random.Next(2) == 0 ? new Sword() : new DoubleSword();
            IEquippable wrapped = WrapWeaponWithRandomModifiers(core);
            return (IWeapon)wrapped;
        }
        /// <summary>
        /// Wraps a weapon with zero or more stacked decorators at level generation time.
        /// </summary>
        private static IEquippable WrapWeaponWithRandomModifiers(Weapon weapon)
        {
            IEquippable item = weapon;
            if (random.Next(3) == 0)
                item = new StrongWeaponModifier(item);
            if (random.Next(3) == 0)
                item = new UnluckyWeaponModifier(item);
            if (random.Next(4) == 0)
                item = new ProtectiveWeaponModifier(item);
            return item;
        }

        /// <summary>
        /// Asynchronously places a number of items at random walkable locations
        /// on the given level.
        /// </summary>
        /// <typeparam name="TItem">The concrete <see cref="Item"/> type.</typeparam>
        /// <param name="level">Level instance to populate.</param>
        /// <param name="count">Total number of items to spawn.</param>
        /// <param name="factory">Function that returns a fresh item instance.</param>
        /// <returns>A task that completes when all items have been placed.</returns>
        /// <remarks>
        /// The placement logic executes on the thread pool because the work is
        /// CPU-bound; callers can await the returned task without blocking the
        /// caller thread.
        /// </remarks>
        public static Task SpawnItemsAsync<TItem>(Level level, int count, Func<TItem> factory)
            where TItem : IItem
        {
            return Task.Run(() =>
            {
                for (int i = 0; i < count; i++)
                {
                    Position pos;
                    do
                    {
                        int x = random.Next(1, level.Width - 1);
                        int y = random.Next(1, level.Height - 1);
                        pos = new Position(x, y);
                    }
                    while (!level.GetTile(pos.X, pos.Y).IsWalkable);

                    level.AddItem(pos, factory());
                }
            });
        }







        /// <summary>
        /// Convenience wrapper that spawns coins.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="count"></param>
        public static Task SpawnCoinsAsync(Level level, int count) =>
            SpawnItemsAsync(level, count, () => new Coin());

        /// <summary>
        /// Convenience wrapper that spawns one-handed swords.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="count"></param>
        public static Task SpawnSwordAsync(Level level, int count) =>
            SpawnItemsAsync(level, count, () => WrapWeaponWithRandomModifiers(new Sword()));

        /// <summary>
        /// Convenience wrapper that spawns two-handed swords.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="count"></param>
        public static Task SpawnDoubleSwordAsync(Level level, int count) =>
            SpawnItemsAsync(level, count, () => WrapWeaponWithRandomModifiers(new DoubleSword()));

        /// <summary>
        /// Convenience wrapper that spawns gold piles.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static Task SpawnGoldAsync(Level level, int count) =>
            SpawnItemsAsync(level, count, () => new Gold());

        /// <summary>
        /// Convenience wrapper that spawns potions.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static Task SpawnPotionsAsync(Level level, int count) =>
            SpawnItemsAsync(level, count, () => new Potion());

        /// <summary>
        /// Conveniece wrapper that spawns Thorns
        /// </summary>
        /// <param name="level"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static Task SpawnThornsAsync(Level level, int count) =>
            SpawnItemsAsync(level, count, () => new Thorn());

        /// <summary>
        /// Spawns golems at random walkable, unoccupied positions, avoiding the default player spawn.
        /// </summary>
        public static Task SpawnGolemAsync(Level level, int count)
        {
            return Task.Run(() =>
            {
                var used = new HashSet<(int X, int Y)>();
                foreach (var g in level.Golems)
                    used.Add((g.Pos.X, g.Pos.Y)); //basically skips these instructions when adding first golem

                int spawned = 0;
                int attempts = 0;
                const int maxAttemptsPerGolem = 500;

                while (spawned < count && attempts < count * maxAttemptsPerGolem)
                {
                    attempts++;
                    int x = random.Next(1, level.Width - 1);
                    int y = random.Next(1, level.Height - 1);

                    if (x == Config.DefaultSpawnX && y == Config.DefaultSpawnY)
                        continue;

                    var tile = level.GetTile(x, y);
                    if (!tile.IsWalkable || tile.IsOccupied)
                        continue;

                    if (!used.Add((x, y)))
                        continue;

                    level.AddGolem(new Golem(new Position(x, y), CreateRandomGolemWeapon()));
                    spawned++;
                }
            });
        }
    }
}
