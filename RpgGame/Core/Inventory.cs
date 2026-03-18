using RpgGame.Character;
using RpgGame.Items;
namespace RpgGame.Core;

/// <summary>
/// Manages the player's inventory system, storing and organizing equippable items.
/// </summary>
/// <remarks>
/// The inventory has a fixed capacity and maintains a LIFO (Last In, First Out) structure.
/// Items are added to the inventory and can be equipped directly to either hand.
/// </remarks>
public class Inventory
{
    /// <summary>
    /// The backing array of equippable items, representing fixed slots.
    /// </summary>
    private IEquippable?[] inventory;

    /// <summary>
    /// The maximum number of items the inventory can hold.
    /// </summary>
    private int _size = 10;

    /// <summary>
    /// The currently selected inventory slot index (0-9).
    /// </summary>
    public int SelectedIndex { get; set; } = 0;

    /// <summary>
    /// Reference to the player who owns this inventory.
    /// </summary>
    private Player _player;

    /// <summary>
    /// Initializes a new instance of the <see cref="Inventory"/> class.
    /// </summary>
    /// <param name="player">The player who owns the inventory.</param>
    /// <param name="size">The maximum capacity of the inventory (ignored, forced to 10).</param>
    public Inventory(Player player, int size)
    {
        _size = 10;
        _player = player;
        inventory = new IEquippable?[_size];
    }

    /// <summary>
    /// Adds an item to the inventory at the selected index if empty, 
    /// otherwise finds the nearest empty slot.
    /// </summary>
    /// <param name="item">The equippable item to add.</param>
    /// <returns>True if the item was successfully added; false if the inventory is full.</returns>
    public bool AddToInventory(IEquippable item)
    {
        // Try to add to selected index first
        if (inventory[SelectedIndex] == null)
        {
            inventory[SelectedIndex] = item;
            return true;
        }

        // Otherwise find nearest empty slot
        for (int i = 0; i < _size; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = item;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Gets the currently selected item without removing it.
    /// </summary>
    /// <returns>The selected item, or null if the slot is empty.</returns>
    public IEquippable? GetSelectedItem()
    {
        return inventory[SelectedIndex];
    }

    /// <summary>
    /// Gets the nth item in the inventory without removing it.
    /// </summary>
    /// <param name="n">The 0-based index of the item to retrieve.</param>
    /// <returns>The nth item in the inventory, or null if n is out of bounds or slot is empty.</returns>
    public IEquippable? GetNItem(int n)
    {
        if (n < 0 || n >= _size) return null;
        return inventory[n];
    }

    /// <summary>
    /// Gets the current number of items in the inventory.
    /// </summary>
    /// <returns>The number of items currently stored in the inventory.</returns>
    public int GetCount()
    {
        int count = 0;
        for (int i = 0; i < _size; i++)
        {
            if (inventory[i] != null) count++;
        }
        return count;
    }

    /// <summary>
    /// Equips the selected item in the inventory to the player's right hand and removes it from inventory.
    /// </summary>
    /// <returns>True if an item was successfully equipped; false if the slot is empty or equip failed.</returns>
    public bool TakeToRight()
    {
        var item = inventory[SelectedIndex];
        if (item == null) return false;

        if (_player.EquipRight(item))
        {
            inventory[SelectedIndex] = null;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Equips the selected item in the inventory to the player's left hand and removes it from inventory.
    /// </summary>
    /// <returns>True if an item was successfully equipped; false if the slot is empty or equip failed.</returns>
    public bool TakeToLeft()
    {
        var item = inventory[SelectedIndex];
        if (item == null) return false;

        if (_player.EquipLeft(item))
        {
            inventory[SelectedIndex] = null;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Gets the maximum size of the inventory.
    /// </summary>
    public int Size => _size;
}
