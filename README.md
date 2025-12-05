# UAS-ARVR

Unity project for AR/VR final assignment with procedural house layout generation.

## Prerequisites

### Required Unity Assets

This project requires the following asset from Unity Asset Store:

- **House Interior Free** by nappin  
  Download from: [Unity Asset Store](https://assetstore.unity.com/packages/3d/props/interior/house-interior-free-258782)

**Installation:**
1. Open the project in Unity Editor
2. Download and import the "House Interior Free" asset from the Asset Store
3. The asset should be imported to `Assets/nappin/` folder

> **Note:** This asset is not included in the repository due to its size (82+ MB) and licensing restrictions. You must download it manually from the Asset Store.

## Project Structure

```
Unity/
├── Assets/
│   ├── Prefabs/          # Room prefabs (Living room, Kitchen, Bedroom, Bathroom)
│   ├── Scripts/
│   │   ├── LayoutGenerator/  # Procedural layout generation logic
│   │   └── Untuk Air/        # Water simulation scripts
│   ├── Scenes/           # Unity scenes
│   └── Settings/         # URP and project settings
├── Packages/             # Unity package dependencies
└── ProjectSettings/      # Project configuration
```

## Features

- **Procedural House Layout Generator**
  - Spawns living room as the central anchor
  - Randomizes placement of kitchen, bedroom, and bathroom (left/top/right positions)
  - Uses hard-coded offsets per room/position for proper wall alignment

## Setup

1. Clone this repository
2. Download and import the required asset (see Prerequisites)
3. Open the project in Unity 2022.3 or later
4. Open `Scenes/SampleScene.unity`
5. Press Play to see the procedural layout generation

## Development

This project uses Universal Render Pipeline (URP) with separate profiles for Mobile and PC platforms.

### Branch Strategy

- `main` - Stable production code
- `feat/*` - Feature branches for new functionality
- `fix/*` - Bug fix branches

## Requirements
- Unity 2022.3 LTS or newer
- Universal Render Pipeline (URP)
- XR Plugin Management

## License

This project is for educational purposes only. Please respect the licenses of all third-party assets used.
