# RpgGame

Console-based RPG game developed for the Object-Oriented Design course.

---

## Overview

This project implements a procedurally generated RPG world with:

- Player movement
- Item system
- Equipment system
- Inventory management
- Sidebar UI rendering

---

## How to Compile

Using `dotnet run` in the `RpgGame` directory.

---
## How to Play

The game is a console-based RPG where you explore a randomly generated room. You can collect items to inventory or currency. Items can also be equipped from the inventory.

### Controls
- **WASD**: Move up, down, left, right
- **F**: Pick up the top item on your current tile
- **Q**: Equip the top inventory item to your left hand
- **E**: Equip the top inventory item to your right hand
- **Shift + Q**: Drop the item from your left hand
- **Shift + E**: Drop the item from your right hand
- **Escape**: Quit the game

### Gameplay Mechanics
- **Movement**: Use WASD to move your character around the level. The level consists of rooms with walls, obstacles, and randomly spawned items.
- **Items**: Items are scattered throughout the level. Walk over them and press F to add them to your inventory. Hovering over the Item also displays the name of the item in the menu.
- **Inventory**: Your inventory holds up to 20 items (can be changed in `Program.cs`). The sidebar shows the first 5 items.
- **Equipment**: You can equip items to your left and right hands. Coins/Gold equipped don't take up inventory space
- **Currency**: Collect coins and gold as you explore. These are displayed in the sidebar.
- **Sidebar**: The right side of the screen shows your character's stats, equipped items, currency, the item on your current tile, and your inventory.


---

## Level Generation

The level is generated using a room-based procedural generator:
- Full floor initialization
- Border wall creation
- Random obstacle clusters
- Random item spawning

---

## Project Structure

- `RpgGame/Core/` – Game state & configuration
- `RpgGame/Generation/` – Map generation logic
- `RpgGame/Items/` – Item and equipment system
- `RpgGame/Character/` – Player & character logic
- `RpgGame/Renderer/` – Console renderer

---

## TODO

- `RpgGame/Renderer/ConsoleRenderer.cs/` - There is a god awful bug to which a bandaid fix for now with ideas for further improvements. For further details check the remarks for the method `GetSidebarContent`.
