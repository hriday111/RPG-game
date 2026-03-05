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
    /// The backing list of equippable items.
    /// </summary>
    private List<IEquippable> inventory = new List<IEquippable>();

    /// <summary>
    /// The maximum number of items the inventory can hold.
    /// </summary>
    private int _size =0;

    /// <summary>
    /// The current number of items in the inventory.
    /// </summary>
    private int _count=0;

    /// <summary>
    /// Reference to the player who owns this inventory.
    /// </summary>
    private Player _player;

    /// <summary>
    /// Initializes a new instance of the <see cref="Inventory"/> class.
    /// </summary>
    /// <param name="player">The player who owns the inventory.</param>
    /// <param name="size">The maximum capacity of the inventory.</param>
    public Inventory(Player player, int size)
    {
        _size = size;
        _player = player;
        inventory = new List<IEquippable>(_size);
    }
    /// <summary>
    /// Adds an item to the inventory.
    /// </summary>
    /// <param name="item">The equippable item to add.</param>
    /// <returns>True if the item was successfully added; false if the inventory is full.</returns>
    public bool AddToInventory(IEquippable item)
    {
        if(_count==_size) {return false;}
        else {_count++; inventory.Add(item); return true;}
    }

    /// <summary>
    /// Gets the top (most recently added) item in the inventory without removing it.
    /// </summary>
    /// <returns>The top item in the inventory, or null if the inventory is empty.</returns>
    public IEquippable?  GetTopItem()
    {
        if(_count==0){return null;}
        else{return inventory[0];}
    }
    /// <summary>
    /// Gets the nth item in the inventory without removing it.
    /// </summary>
    /// <param name="n">The 0-based index of the item to retrieve.</param>
    /// <returns>The nth item in the inventory, or null if n is out of bounds.</returns>
    public IEquippable?  GetNItem(int n)
    {
        if(_count==0 || n>_count){return null;}
        else{return inventory[n];}
    }

    /// <summary>
    /// Gets the current number of items in the inventory.
    /// </summary>
    /// <returns>The number of items currently stored in the inventory.</returns>
    public int GetCount()
    {
        return _count;
    }
    /// <summary>
    /// Equips the top item in the inventory to the player's right hand and removes it from inventory.
    /// </summary>
    /// <returns>True if an item was successfully equipped; false if the inventory is empty.</returns>
    public bool TakeToRight()
    {
        if(_count==0) {return  false;}
        else {
            if(_player.EquipRight(inventory[0]))
            {
                inventory.RemoveAt(0); _count--;  return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Equips the top item in the inventory to the player's left hand and removes it from inventory.
    /// </summary>
    /// <returns>True if an item was successfully equipped; false if the inventory is empty.</returns>
    public bool TakeToLeft()
    {
        if(_count==0) {return  false;}
        else 
        {
            if(_player.EquipLeft(inventory[0]))
            {
                inventory.RemoveAt(0); _count--;  return true;
            }
            return false;
        }
    }
}