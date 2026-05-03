# RpgGame

![.NET CI](https://github.com/hriday111/RPG-game/actions/workflows/dotnet-ci.yml/badge.svg)
![CodeQL Analysis](https://github.com/hriday111/RPG-game/actions/workflows/codeql.yml/badge.svg)
![Docs](https://github.com/hriday111/RPG-game/actions/workflows/docs.yml/badge.svg)

Console-based RPG game developed for the Object-Oriented Design course.

# Documentation
[`RpgGame`](https://hriday111.github.io/RPG-game/)

---

## CI/CD Pipeline
This project includes a fully automated CI/CD pipeline:
- **Continuous Integration**: Every push builds the code, runs unit tests, and enforces coding standards (`dotnet format`).
- **Security Analysis**: GitHub CodeQL scans the codebase for potential vulnerabilities.
- **Automated Documentation**: Doxygen documentation is automatically generated and deployed to `gh-pages` on every update to `main`.

## Testing
We use **xUnit** for unit testing. To run tests locally:
```bash
dotnet test
```
Current tests cover:
- Combat round resolution against golems and mages (damage, armor, defeat).
- Level combat integration (enemy lookup, orthogonal melee messaging).
- Player health mechanics where applicable alongside combat tests.

---

## New Additions:

New Potions and Thorns have been added. Trying to pick up Thorns causes 50 damage, and picking up potions result in increase of health by 20 points. 

---

## Overview

This project implements a procedurally generated RPG world with:

- Player movement on themed dungeons
- Item system
- Equipment system
- Inventory management
- Sidebar UI rendering (includes a short dungeon intro line when a theme defines one)


---

## How to Compile

Using `dotnet run` in the `RpgGame` directory.

---

## How to Play

The game is a console-based RPG where you explore a randomly generated **dungeon**. Each run picks a **theme** (weighted random), which influences layout, loot, enemies, and a thematic intro shown in the sidebar. You collect items into inventory or as currency and equip gear from inventory.

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
- **Sidebar**: The right side of the screen shows your character's stats, a one-line dungeon intro (from the active theme), equipped items, currency, combat feedback, selected inventory slots, and item details.


---

## Level Generation

Generation combines the **Strategy** and **Builder** patterns with explicit **dungeon themes**:

- **Themes**: Before the map is built, a `DungeonThemeKind` is drawn using `DungeonThemePicker` (**Basic** and **Treasure** at 40% each; **Library** and **Healing** at 10% each—see `RpgGame/Generation/Themes/DungeonThemePicker.cs`).
- **`DungeonThemeProfile` / `DungeonThemeCatalog`**: For each theme, a profile holds the **intro line**, **item and weapon spawn counts**, a **guaranteed artifact** factory (`Func<IItem>`), and an ordered list of **`IEnemySpawnStep`** entries (spawn steps are composed in sequence; themes can combine multiple enemy types by adding multiple steps—see `RpgGame/Generation/Themes/DungeonThemeCatalog.cs` and `RpgGame/Generation/Enemies/`).
- **Procedural steps**: Each `IDungeonProcedure` handles one concern (walls, rooms, paths, loot, weapons, artifact placement, enemies).
- **`DungeonBuilder`**: Chains procedures and passes a `DungeonContext` carrying the chosen `DungeonThemeKind`.
- **`DungeonStrategyCatalog`**: Maps theme kind to a concrete `IDungeonStrategy` **without switch statements** (`RpgGame/Generation/Strategies/DungeonStrategyCatalog.cs`). Strategies differ mainly in geometric parameters (central room size, chamber count):
  - **`BasicDungeonStrategy`** — balanced “grounds-style” dungeon.
  - **`TreasureDungeonStrategy`** — larger central vault and more chambers; profile favors coins/gold and mages as enemies.
  - **`LibraryDungeonStrategy`** — narrower core and more chambers; profile favors crystal orbs plus a thematic artifact.
  - **`HealingDungeonStrategy`** — calmer proportions; profile emphasizes potions plus a thematic artifact.

**Sandbox**: `DungeonSandboxStrategy` still uses one large starter room but applies the same themed profile for loot, artifact, and enemy steps.

Heavy placement work runs **asynchronously** via `Task.Run()` inside helpers such as `MapSpawnHelper`, so generation stays non-blocking.

For detailed API documentation, see the [RpgGame.Generation namespace documentation](https://hriday111.github.io/RPG-game/namespaceRpgGame_1_1Generation.html).

---

## Project Structure

- [`RpgGame/Core/`](https://hriday111.github.io/RPG-game/namespaceRpgGame_1_1Core.html) – Game state & configuration (`Level`, including optional `DungeonIntro`)
- [`RpgGame/Generation/`](https://hriday111.github.io/RPG-game/namespaceRpgGame_1_1Generation.html) – Map generation (procedures, builder, themed strategies & catalogs under `Themes/`, `Strategies/`, `Enemies/`)
- [`RpgGame/Items/`](https://hriday111.github.io/RPG-game/namespaceRpgGame_1_1Items.html) – Item and equipment system
- [`RpgGame/Character/`](https://hriday111.github.io/RPG-game/namespaceRpgGame_1_1Character.html) – Player & character logic
- [`RpgGame/Renderer/`](https://hriday111.github.io/RPG-game/namespaceRpgGame_1_1Rendering.html) – Console renderer

---

## TODO
- Sucks that you have to pick up thorns to get damage, Ideally it would be great such that if a player steps on a tile with harmful items he gets the damage. I tried to implement it but got really lazy so thats another problem for future me. 

This feature could be adapted to coins and golds also.
- In Player.cs I don't really have a game over screen. So you could just get your health down to 0 and continue playing like nothings wrong. Gotta fix that but let that just be a future me problem for now.