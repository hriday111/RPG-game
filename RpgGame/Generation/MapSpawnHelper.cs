using System;
using System.Threading.Tasks;
using RpgGame.Core;
using RpgGame.Items;
using RpgGame.Character;

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
        public static Task SpawnCoinsAsync(Level level, int count) =>
            SpawnItemsAsync(level, count, () => new Coin());

        /// <summary>
        /// Convenience wrapper that spawns one-handed swords.
        /// </summary>
        public static Task SpawnSwordAsync(Level level, int count) =>
            SpawnItemsAsync(level, count, () => new Sword());

        /// <summary>
        /// Convenience wrapper that spawns two-handed swords.
        /// </summary>
        public static Task SpawnDoubleSwordAsync(Level level, int count) =>
            SpawnItemsAsync(level, count, () => new DoubleSword());

        /// <summary>
        /// Convenience wrapper that spawns gold piles.
        /// </summary>
        public static Task SpawnGoldAsync(Level level, int count) =>
            SpawnItemsAsync(level, count, () => new Gold());
    }
}