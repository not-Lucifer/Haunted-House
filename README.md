# Haunted House

A 3D isometric stealth game built in Unity. You play a character trapped in a haunted house and must sneak past patrolling ghosts and watchful gargoyles to reach the exit. If any of them spots you, you're caught and the level restarts.

The project is based on Unity Learn's **3D Stealth Game** tutorial. I extended it with a second level, multi-level progression and footstep audio.

---

## Gameplay

- **Goal:** Get from the start of each level to the exit without being seen.
- **Enemies:**
  - **Ghosts** patrol set routes through the house using waypoints.
  - **Gargoyles** stay in one place and watch a fixed area.
- **Detection:** Every enemy has a field-of-view trigger. When you're inside it and nothing blocks the line of sight between you, you're caught.
- **Win / Lose:**
  - Reach the exit: the "escaped" screen fades in and the next level loads. Clearing the last level ends the game.
  - Get caught: the "caught" screen fades in and the current level restarts.

### Controls

| Key       | Action         |
|-----------|----------------|
| `W` / `S` | Move forward / back |
| `A` / `D` | Move left / right |

---

## Tech Stack

| Area            | Technology |
|-----------------|------------|
| Engine          | Unity **6000.3.9f1** (Unity 6) |
| Language        | C# |
| Rendering       | Universal Render Pipeline (URP) 17.3 |
| Post-processing | URP Volume framework (bloom, vignette, color grading, etc.) |
| Camera          | Cinemachine 3.1 |
| Input           | Unity Input System 1.18 (`InputAction` with a 2D vector WASD binding) |
| UI              | UI Toolkit (UXML + `UIDocument`) for the main menu and end screens |
| Physics         | Rigidbody movement, trigger colliders, raycasting |
| Animation       | Mecanim Animator (idle/walk states driven by a bool parameter) |
| Audio           | AudioSource: 2D ambient music, 3D positional SFX, footsteps, win/lose stings |
| Build targets   | Windows (Standalone) and WebGL |
| Version control | Git |

---

## Project Structure

```
Assets/
├── _3DStealthGame/
│   ├── Art/            # Models, animations, materials, textures, shaders, fonts, post-processing profiles
│   ├── Audio/          # Ambience, footsteps, ghost, candle, buzzing light, win/lose SFX
│   ├── Prefabs/
│   │   ├── Characters/ # Player, Ghost, Gargoyle
│   │   ├── Levels/     # Level A, Level B, Level C
│   │   ├── Environment/
│   │   ├── VFX/
│   │   └── Point of View.prefab   # Enemy vision cone (trigger + Observer)
│   ├── Scenes/
│   │   ├── Main.unity  # Level 1
│   │   └── Level2.unity
│   ├── Scripts/        # Gameplay code (see below)
│   ├── Settings/       # URP asset, renderer, lighting settings
│   ├── UI/             # MainUI.uxml (End/Caught screens), PanelSettings
│   └── Tutorial_Demo/  # Reference demo scenes and scripts (incl. MainMenu)
└── Scenes/
```

### Core Scripts

| Script | Responsibility |
|--------|----------------|
| [`PlayerMovement.cs`](Assets/_3DStealthGame/Scripts/PlayerMovement.cs) | Reads WASD input, rotates and moves the player's Rigidbody in `FixedUpdate`, drives the `IsWalking` animation and plays footstep audio. |
| [`WayPointPatrol.cs`](Assets/_3DStealthGame/Scripts/WayPointPatrol.cs) | Moves an enemy along a looping list of waypoints and turns it to face its direction of travel. |
| [`Observer.cs`](Assets/_3DStealthGame/Scripts/Observer.cs) | Tracks when the player enters or leaves an enemy's vision trigger, then raycasts to confirm line of sight before reporting a catch. |
| [`GameEnding.cs`](Assets/_3DStealthGame/Scripts/GameEnding.cs) | Handles the exit trigger and catch events: fades in the matching UI screen, plays a one-shot sound, then restarts, loads the next level or quits. |
| [`MainMenu.cs`](Assets/_3DStealthGame/Tutorial_Demo/Demo_Scripts/Bonus%20Features/MainMenu.cs) | Connects the UI Toolkit Start and Exit buttons to scene loading and quitting. |

---

## Game Workflow

```
            ┌──────────────┐
            │  Main Menu   │  (build index 0)
            └──────┬───────┘
          Start    │    Exit ──► Application.Quit()
                   ▼
            ┌──────────────┐
     ┌─────►│   Level 1    │  Main.unity (build index 1)
     │      └──────┬───────┘
     │             │
     │   ┌─────────┴──────────┐
     │   │                    │
     │ Caught by enemy    Reached exit
     │   │                    │
     │   ▼                    ▼
     │ Fade "Caught"      Fade "Escaped"
     │ + lose SFX         + win SFX
     │   │                    │
     └───┘ reload level       ▼
                       ┌──────────────┐
                ┌─────►│   Level 2    │  Level2.unity (build index 2)
                │      └──────┬───────┘
                │   Caught    │    Reached exit
                └─────────────┘         │
                                        ▼
                               Last level cleared
                               → Application.Quit()
```

### Runtime flow, frame by frame

1. **Input → Movement.** `PlayerMovement` reads the `MoveAction` vector, normalizes it, rotates toward it with `Vector3.RotateTowards`, and moves with `Rigidbody.MovePosition`, all in `FixedUpdate` so physics stays stable.
2. **Animation and audio.** If there's any input, `IsWalking` is set to true, which switches the Animator from Idle to Walk and starts the footstep audio.
3. **Enemy patrol.** Each ghost's `WayPointPatrol` moves it toward its current waypoint. Within 0.1 units it moves on to the next waypoint and loops back to the first after the last.
4. **Detection.** When the player enters an enemy's **Point of View** trigger, `Observer` starts raycasting toward the player (aimed slightly upward at the torso). If the ray hits the player first, rather than a wall, it calls `GameEnding.CaughtPlayer()`.
5. **Level end.** `GameEnding.Update` raises the opacity of the matching UI Toolkit screen over `fadeDuration`, plays its sound once, holds the screen for `displayImageDuration`, and then:
   - **Caught:** reloads the active scene.
   - **Escaped:** loads `buildIndex + 1`, or quits after the last level.

---

## Concepts Learned

**Unity fundamentals**
- Organizing a project with prefabs, nested prefabs and level prefabs
- The MonoBehaviour lifecycle: `Awake`, `OnEnable`, `Start`, `Update` and `FixedUpdate`, and when to use each
- Exposing tuning values in the Inspector with `[SerializeField]` instead of public fields

**Physics and movement**
- Moving characters with a Rigidbody (`MovePosition` / `MoveRotation`) instead of setting the Transform directly
- Smooth turning with `Vector3.RotateTowards` and `Quaternion.LookRotation`
- Trigger colliders (`OnTriggerEnter` / `OnTriggerExit`) for detection zones and the level exit
- Raycasting for line-of-sight checks, so walls block an enemy's view

**Enemy AI**
- Waypoint-based patrolling with modular index looping
- Splitting enemy logic into separate parts: movement (`WayPointPatrol`) and perception (`Observer`)

**Input**
- The new Input System: setting up an `InputAction` with a 2D composite (WASD) binding and reading it as a `Vector2`

**Animation**
- Animator Controllers, states and transitions driven by parameters (`IsWalking`)

**Camera and visuals**
- A Cinemachine virtual camera that follows the player from an isometric angle
- URP setup, baked lighting settings and post-processing Volumes for a spooky look

**Audio**
- 2D ambient audio compared with 3D spatial audio (positional enemy sounds, candles, buzzing lights)
- Footstep audio tied to movement state
- One-shot win and lose sounds with a guard flag so they play only once

**UI**
- Building screens with UI Toolkit (UXML) and querying elements from C# with `rootVisualElement.Q<T>()`
- Fading screens in and out by animating `style.opacity`
- Connecting button `clicked` events to game actions

**Game flow**
- Managing scenes with `SceneManager`: reloading the current level, moving to the next build index, and the Build Settings scene list
- Quitting the game with `Application.Quit()`

**Workflow**
- Building in small steps and committing each feature separately: player movement, camera, static enemies, dynamic enemies, game ending, audio, a new level, footsteps
- Building for multiple platforms (Windows and WebGL)

---

## Getting Started

### Requirements
- Unity Hub
- Unity Editor **6000.3.9f1** (or a compatible Unity 6 version)

### Run in the Editor
1. Clone the repository:
   ```bash
   git clone https://github.com/not-Lucifer/Haunted-House
   ```
2. Open the project folder in **Unity Hub**, using Unity 6000.3.9f1.
3. Open `Assets/_3DStealthGame/Tutorial_Demo/Demo_Scenes/MainMenu.unity`.
4. Press **Play**.

### Build
1. Go to **File → Build Profiles**.
2. Check that the scene list is in this order:
   1. `MainMenu`
   2. `Main` (Level 1)
   3. `Level2`
3. Choose **Windows** or **Web** and click **Build**.

---

## Development History

| Milestone | Description |
|-----------|-------------|
| Player Movement and Animations | Input System movement, Rigidbody physics, walk/idle animation |
| Camera and Post-Processing | Cinemachine follow camera and URP post-processing |
| Static Enemies | Gargoyles with vision cones and line-of-sight detection |
| Dynamic Enemies | Ghosts that patrol between waypoints |
| Game Ending Logic | Win and caught screens with fades and scene reload |
| 2D Ambient Audio | Background ambience and SFX |
| New Level | Level 2 and loading the next level |
| Footsteps | Player footstep audio |

---

## Credits

- Based on the **3D Stealth Game: Haunted House** tutorial from [Unity Learn](https://learn.unity.com/).
- The art, animation and audio assets come from the Unity Learn tutorial package.
- Developed by **Aman Singh**.
