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