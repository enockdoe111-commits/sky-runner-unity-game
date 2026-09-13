# Complete Sky Runner Setup Guide for Unity

## Step 1: Clone the Repository into Unity

### Option A: Clone Using Git

1. **Open Terminal/Command Prompt** in your desired location
2. **Run this command:**
   ```bash
   git clone https://github.com/enockdoe111-commits/sky-runner-unity-game.git
   ```
3. **Open Unity Hub**
4. Click **"Add project"** → Select the cloned folder
5. **Select Unity version 2020.3 LTS or newer** → Click **Open**

### Option B: Download as ZIP

1. Go to https://github.com/enockdoe111-commits/sky-runner-unity-game
2. Click **"Code"** → **"Download ZIP"**
3. **Extract the ZIP folder** to your desired location
4. **Open Unity Hub** → Click **"Add project"** → Select the extracted folder
5. **Select Unity version 2020.3 LTS or newer** → Click **Open**

---

## Step 2: Create the Project Folder Structure

Once the project is open in Unity, create these folders in the **Assets** folder:

```
Assets/
├── Scripts/
│   ├── Player/
│   ├── Game/
│   ├── World/
│   ├── Collectibles/
│   ├── Camera/
│   ├── Audio/
│   ├── UI/
│   └── Utilities/
├── Scenes/
├── Prefabs/
│   ├── Obstacles/
│   ├── Coins/
│   ├── PowerUps/
│   └── TrackSegments/
├── Materials/
├── Models/
├── Audio/
│   ├── Music/
│   └── SFX/
├── Animations/
└── UI/
```

**How to create folders:**
1. In the **Project window** (bottom left), right-click in the Assets folder
2. Select **Create** → **Folder**
3. Name the folder and repeat for each directory

---

## Step 3: Import All Scripts

All scripts are already in the repository under `Assets/Scripts/`. They should appear automatically in your project.

**Verify scripts are imported:**
1. In the Project window, expand **Assets/Scripts/**
2. You should see all the C# files organized in subfolders
3. If scripts don't appear, manually drag them from the cloned folder into the correct Script subfolders

---

## Step 4: Create the Main Scene (GamePlay)

1. **Create a new scene:**
   - Click **File** → **New Scene**
   - Choose **3D Scene**
   - Save it as `GamePlay` in **Assets/Scenes/**

2. **Set up the scene hierarchy:**
   - Right-click in the Hierarchy and create these GameObjects:
     - **GameManager** (empty GameObject)
     - **Player** (empty GameObject, position: 0, 1, 0)
     - **World** (empty GameObject)
     - **Camera** (delete default camera, create new)
     - **Canvas** (UI)

3. **Add scripts to GameObjects:**
   - Select **GameManager** → In Inspector, drag `GameManager.cs` into it
   - Select **Player** → Drag `PlayerController.cs` into it
   - Create a **Track** child under **World** → Add `TrackGenerator.cs`
   - Create an **ObstacleSpawner** child under **World** → Add `ObstacleSpawner.cs`

---

## Step 5: Create the Player

1. **Create Player GameObject:**
   - Right-click in Hierarchy → **3D Object** → **Capsule**
   - Rename to **PlayerCharacter**
   - Set position to (0, 1, 0)
   - Scale to (0.5, 1, 0.5)

2. **Add Components:**
   - **Rigidbody:** Click Add Component → Physics → Rigidbody
   - Set **Mass** to 1
   - Set **Drag** to 5
   - Set **Angular Drag** to 5
   - **Uncheck** "Use Gravity" (we handle it in PlayerController)
   - **Collider:** Already has CapsuleCollider by default

3. **Add Physics Material:**
   - Create a new Physics Material (right-click in Assets → Physics Material)
   - Name it `PlayerPhysicsMaterial`
   - Set **Friction** to 0
   - Set **Bounciness** to 0
   - Drag this into the CapsuleCollider's **Material** field

4. **Add Animator:**
   - Add Component → Animation → Animator
   - Create a new Animator Controller: right-click in Assets/Animations → Animator Controller
   - Name it `PlayerAnimator`
   - Assign it to the Animator component

5. **Tag the Player:**
   - Select PlayerCharacter → In Inspector, change **Tag** to "Player"
   - If "Player" tag doesn't exist, click the dropdown → "Add Tag" → Create "Player"

6. **Make it a Prefab:**
   - Drag **PlayerCharacter** from Hierarchy to **Assets/Prefabs/**
   - This creates a reusable player prefab

---

## Step 6: Create Obstacles

1. **Create Barrier Obstacle:**
   - Right-click in Hierarchy → **3D Object** → **Cube**
   - Rename to **Barrier**
   - Scale to (2, 2, 0.5)
   - Add **BoxCollider** (already has one by default)
   - **Uncheck** "Is Trigger" on the collider
   - Add a script: Create new script `Obstacle.cs`:

```csharp
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Trigger collision in PlayerController
        }
    }
}
```

   - Drag **Barrier** to **Assets/Prefabs/Obstacles/** to create a prefab

2. **Create other obstacles similarly:**
   - **MovingWall:** Cube that moves side to side
   - **RotatingSpinner:** Cube that rotates
   - **Gap:** Invisible trigger below the track

---

## Step 7: Create Coins

1. **Create a Coin:**
   - Right-click in Hierarchy → **3D Object** → **Cylinder**
   - Rename to **Coin**
   - Scale to (0.3, 0.1, 0.3)
   - Rotate it 90 degrees on the X axis
   - Add a **SphereCollider**
   - **Check** "Is Trigger"
   - Create material: right-click in Assets/Materials → Material
   - Name it `CoinMaterial`
   - Set **Metallic** to 1, **Smoothness** to 1, **Color** to gold/yellow
   - Drag material onto the Coin

2. **Add Coin Script:**
   - Select Coin → Add Component → find `Coin.cs`
   - Tag the coin as "Coin"
   - Drag **Coin** to **Assets/Prefabs/Coins/** to create prefab

---

## Step 8: Create the Track

1. **Create Track Segment:**
   - Right-click in Hierarchy → **3D Object** → **Plane**
   - Rename to **TrackSegment**
   - Scale to (3, 1, 20)
   - Tag it as "Ground"
   - Add Physics Material with high friction

2. **Drag to Prefabs:**
   - Drag **TrackSegment** to **Assets/Prefabs/TrackSegments/**

---

## Step 9: Create the Canvas (UI)

1. **Create Canvas:**
   - Right-click in Hierarchy → **UI** → **Canvas**
   - Name it **GameplayUI**

2. **Add UI Elements:**
   - **Score Text:** Right-click Canvas → **UI** → **Text (Legacy)**
     - Name it `ScoreText`
     - Set Font Size to 40
     - Anchor to top-left
     - Set text to "Score: 0"
   
   - **Coins Text:** Duplicate ScoreText, rename to `CoinsText`, position below
   
   - **Distance Text:** Duplicate again, rename to `DistanceText`
   
   - **Pause Button:** Right-click Canvas → **UI** → **Button**
     - Name it `PauseButton`
     - Anchor to top-right
     - Change text to "PAUSE"

3. **Add UIManager Script:**
   - Select Canvas → Add Component → find `UIManager.cs`

---

## Step 10: Create Audio Manager

1. **Create AudioManager GameObject:**
   - Right-click in Hierarchy → Create empty GameObject
   - Rename to **AudioManager**
   - Add Component → Audio → Audio Source

2. **Add AudioManager Script:**
   - Select AudioManager → Add Component → find `AudioManager.cs`

3. **Import Audio Files:**
   - Place audio files in **Assets/Audio/Music/** and **Assets/Audio/SFX/**
   - Assign in the AudioManager Inspector

---

## Step 11: Create Camera Follow System

1. **Select Main Camera:**
   - In Hierarchy, click on **Main Camera**
   - Add Component → find `CameraFollow.cs`
   - Assign the **Player** to the target field

2. **Position Camera:**
   - Set Position to (0, 3, -5) relative to player
   - Adjust as needed for desired view

---

## Step 12: Set Up Input System

1. **Create InputManager:**
   - Right-click in Hierarchy → Create empty GameObject
   - Rename to **InputManager**
   - Add Component → find `InputManager.cs`

---

## Step 13: Final Setup Checklist

- [ ] All scripts imported in correct folders
- [ ] GamePlay scene created with all GameObjects
- [ ] Player created with Rigidbody and Animator
- [ ] Obstacles created as prefabs
- [ ] Coins created as prefabs
- [ ] Track segments created as prefabs
- [ ] Canvas with UI elements created
- [ ] AudioManager set up with audio files
- [ ] Camera follows player
- [ ] InputManager handles touch/keyboard
- [ ] All scripts have correct references assigned in Inspector

---

## Step 14: Test in Editor

1. **Press Play** in the Unity Editor
2. **Test keyboard controls:**
   - Press **A/D** to move left/right
   - Press **Space** to jump
   - Press **Down Arrow** to slide
   - Press **Escape** to pause
3. **Verify:**
   - Player moves forward automatically
   - UI updates score and coins
   - Obstacles and coins spawn
   - Collisions work correctly

---

## Step 15: Build for Android

1. **Go to File** → **Build Settings**
2. **Click "Add Open Scenes"** to add GamePlay scene
3. **Switch Platform** to Android
4. **Configure Player Settings:**
   - Set **Company Name**
   - Set **Product Name** to "Sky Runner"
   - Set **Package Name** to "com.yourname.skyrunner"
   - Set **Minimum API Level** to 24 (Android 7.0)

5. **Click "Build and Run"** to create APK
6. **Connect Android device** via USB with Developer Mode enabled
7. APK will be installed automatically

---

## Troubleshooting

### Scripts show as Missing
- **Solution:** Make sure scripts are in the correct folders under Assets/Scripts/

### Player doesn't move
- Check **PlayerController.cs** is attached to Player GameObject
- Verify Rigidbody is not kinematic

### UI doesn't update
- Assign Canvas references in UIManager script
- Verify all text fields are connected

### No audio
- Check audio files are in Assets/Audio/
- Verify AudioManager has audio clips assigned
- Check volume is not muted

### Game too easy/hard
- Adjust constants in **GameConstants.cs**
- Increase/decrease PLAYER_FORWARD_SPEED
- Modify SCORE_FOR_DIFFICULTY_INCREASE

---

**Next Step:** Open the project in Unity and follow these instructions step-by-step!
