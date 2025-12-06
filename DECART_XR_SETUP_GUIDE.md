# Decart XR API - Samurai Slicing Mechanic Setup Guide

## What You've Built

A standalone samurai slicing game mechanic with:
- **Decart XR API**: Core scoring system (Timing + Hasuji + Velocity)
- **Katana Controller**: XR controller tracking and slash detection
- **Sliceable Objects**: Objects that respond to cuts with scoring
- **Object Spawner**: Throws objects at the player
- **Feedback UI**: Japanese-style visual feedback system
- **Combo System**: Multipliers up to 4x in "EDGELORD MODE"

---

## Step-by-Step Setup in Unity

### Step 1: Create the Scene

1. **Open Unity** (wait for 6000.2.10f1 to finish installing, or use current version)
2. **Create a new scene**:
   - File > New Scene
   - Choose "Basic (URP)" or "3D"
   - Save as `Assets/Scenes/SamuraiSlicing.scene`

### Step 2: Set Up XR Rig

1. **Delete the default Main Camera**

2. **Add XR Origin**:
   - Right-click in Hierarchy > XR > XR Origin (Action-based)
   - This creates the VR camera rig

3. **Configure for Meta Quest**:
   - Select XR Origin
   - Ensure "Camera Y Offset" is set appropriately (1.36m is default standing height)

### Step 3: Create the Katana

1. **Create Katana GameObject**:
   ```
   Hierarchy > Right-click XR Origin > Create Empty
   Name: "Katana"
   ```

2. **Add Katana Blade Visual**:
   ```
   Right-click Katana > 3D Object > Cylinder
   Name: "Blade"
   Transform:
     - Scale: (0.05, 0.4, 0.05)  // Thin blade shape
     - Rotation: (90, 0, 0)       // Point forward
     - Position: (0, 0, 0.4)      // Extend from handle
   ```

3. **Add Trail Renderer to Blade**:
   - Select Blade
   - Add Component > Trail Renderer
   - Settings:
     - Time: 0.5
     - Width: Start = 0.1, End = 0
     - Materials: Use Default-Particle material (or create a glowing trail material)
     - Color: Gradient from white to transparent

4. **Add Collider to Blade**:
   - Add Component > Box Collider
   - Set as Trigger: ✓
   - Size: (0.1, 0.8, 0.1)

5. **Add KatanaController Script**:
   - Select "Katana" (parent object)
   - Add Component > Katana Controller
   - Assign References:
     - Blade Transform: Drag "Blade" object here
     - Slash Trail: Drag Trail Renderer from Blade
     - Controller Node: Right Hand

### Step 4: Create Decart XR API Manager

1. **Create Manager Object**:
   ```
   Hierarchy > Create Empty
   Name: "DecartXRAPI"
   ```

2. **Add Script**:
   - Add Component > Decart XRAPI
   - Configure thresholds (defaults are good to start)

### Step 5: Create Sliceable Object Prefab

1. **Create Object**:
   ```
   Hierarchy > 3D Object > Sphere
   Name: "SliceableObject"
   Scale: (0.2, 0.2, 0.2)
   ```

2. **Configure Components**:
   - **Rigidbody**: Add Component > Rigidbody
     - Use Gravity: ✓
     - Mass: 0.5

   - **Collider**:
     - Is Trigger: ✓

   - **Material**:
     - Create new material (Assets > Create > Material)
     - Name: "SliceableMat"
     - Color: Orange or Red
     - Assign to object

3. **Add SliceableObject Script**:
   - Add Component > Sliceable Object
   - Base Points: 100

4. **Create Prefab**:
   - Drag "SliceableObject" from Hierarchy to Project window (Assets folder)
   - Delete from Hierarchy (we'll spawn it dynamically)

### Step 6: Create Object Spawner

1. **Create Spawner Object**:
   ```
   Hierarchy > Create Empty
   Name: "ObjectSpawner"
   Position: (0, 2, 3)  // In front of and above player
   ```

2. **Add Object Spawner Script**:
   - Add Component > Object Spawner
   - Assign References:
     - Object Prefab: Drag SliceableObject prefab here
     - Target Position: Drag XR Origin's Camera here
   - Settings:
     - Spawn Interval: 2 seconds
     - Spawn Radius: 2 meters
     - Launch Force: 5

### Step 7: Create UI Canvas

1. **Create World Space Canvas**:
   ```
   Hierarchy > UI > Canvas
   Name: "FeedbackCanvas"
   ```

2. **Configure Canvas**:
   - Render Mode: World Space
   - Position: (0, 2, 2)  // In front of player
   - Scale: (0.01, 0.01, 0.01)
   - Width: 800, Height: 600

3. **Add Feedback Text**:
   ```
   Right-click FeedbackCanvas > UI > Text - TextMeshPro
   Name: "FeedbackText"
   ```
   - Font Size: 72
   - Alignment: Center/Middle
   - Color: White

4. **Add Score Text**:
   ```
   Right-click FeedbackCanvas > UI > Text - TextMeshPro
   Name: "ScoreText"
   ```
   - Anchor: Top-Left
   - Font Size: 48

5. **Add Combo Text**:
   ```
   Right-click FeedbackCanvas > UI > Text - TextMeshPro
   Name: "ComboText"
   ```
   - Anchor: Top-Right
   - Font Size: 36

6. **Add SamuraiFeedbackUI Script**:
   - Select FeedbackCanvas
   - Add Component > Samurai Feedback UI
   - Assign all text references

### Step 8: Connect Everything

1. **Update SliceableObject Script**:
   - Select your SliceableObject prefab
   - In SliceableObject component, you can add:
     - Slice Particles: Optional particle effect prefab
     - Slice Sound: Optional audio clip

2. **Wire Up Feedback**:
   - We need to connect the scoring to the UI
   - See "Integration Code" section below

---

## Integration Code

Add this to **SliceableObject.cs** after calculating the score (around line 70):

```csharp
// Show feedback on UI
SamuraiFeedbackUI feedbackUI = FindObjectOfType<SamuraiFeedbackUI>();
if (feedbackUI != null)
{
    feedbackUI.ShowSlashFeedback(result, finalScore);
}
```

---

## Building and Testing

### Test in Unity Editor (Play Mode)

1. Press **Play** in Unity
2. Use XR Device Simulator (should appear automatically)
3. Hold Right Mouse and move to swing the "katana"
4. Objects should spawn and fly toward you

### Build for Meta Quest 3

1. **Update Build Settings**:
   - File > Build Settings
   - Remove SampleScene, add SamuraiSlicing scene
   - Ensure Android is selected

2. **Build APK**:
   - Click "Build and Run"
   - Save as: `Builds/EdgelordSamuraiXR.apk`

3. **Sideload to Quest 3**:
   ```bash
   cd "c:\Users\caity\My project"
   adb install -r Builds/EdgelordSamuraiXR.apk
   ```

4. **Launch on Quest**:
   - Put on headset
   - Go to Apps > Unknown Sources
   - Launch "Hello World VR" (or whatever your product name is)

---

## Understanding the Decart XR API

### The Three Scoring Metrics

1. **Timing (0-40 points)**:
   - Perfect: ±0.1s from beat = 40 points
   - Good: ±0.25s = 20-40 points
   - Measures rhythm accuracy

2. **Hasuji / Edge Alignment (0-40 points)**:
   - Perfect: <15° blade alignment = 40 points
   - Good: <35° = 20-40 points
   - Measures sword technique (blade must face cut direction)

3. **Velocity (0-20 points)**:
   - Minimum: 2 m/s to register cut
   - Perfect: 5+ m/s = 20 points
   - Measures swing power

### Combo System

- Combo builds with successful cuts (score ≥30)
- Combo breaks after 3 seconds of no cuts
- Multipliers:
  - 5+ combo: 1.5x
  - 10+ combo: 2x
  - 20+ combo: 4x "EDGELORD MODE"

### Rank System

- **EDGELORD**: 95-100 points (絶対領域！)
- **MASTER**: 85-94 points (神業！)
- **SAMURAI**: 70-84 points (見事！)
- **WARRIOR**: 50-69 points (良い！)
- **NOVICE**: 30-49 points (頑張れ！)
- **MISS**: <30 points (残念...)

---

## Customization Ideas

1. **Add Music/Rhythm**:
   - Use Unity's audio system to play music
   - Calculate beat times
   - Spawn objects on the beat

2. **Different Object Types**:
   - Fruit, bamboo, enemies
   - Different point values
   - Special objects that give bonuses

3. **Visual Effects**:
   - Slice particle effects (sparks, cherry blossoms)
   - Screen shake on good cuts
   - Slow-motion on perfect cuts

4. **Sound Effects**:
   - "Whoosh" for sword swings
   - Different slice sounds based on score
   - Japanese voice callouts for ranks

---

## Troubleshooting

### Objects not being sliced:
- Make sure Katana has a Collider set to Trigger
- Check that KatanaController is detecting slashes (watch debug logs)
- Verify SliceableObject has is Trigger enabled

### No UI showing:
- Check Canvas is in World Space mode
- Verify text components are assigned in SamuraiFeedbackUI
- Make sure Canvas is positioned in front of camera

### Katana not moving:
- Verify XR Plug-in Management is configured for Oculus
- Check controller node is set to "Right Hand"
- Test with XR Device Simulator in editor

---

## Next Steps

1. Test the basic mechanic
2. Add visual/audio polish
3. Create different object types
4. Implement rhythm/beat system
5. Add multiplayer (Photon Fusion 2) later

---

## File Structure

```
Assets/
├── Scripts/
│   └── DecartXR/
│       ├── DecartXRAPI.cs           # Core scoring engine
│       ├── KatanaController.cs       # Controller tracking
│       ├── SliceableObject.cs        # Objects that can be cut
│       ├── ObjectSpawner.cs          # Spawns objects
│       └── SamuraiFeedbackUI.cs      # Visual feedback
├── Scenes/
│   └── SamuraiSlicing.scene         # Main test scene
└── Prefabs/
    └── SliceableObject.prefab       # Sliceable object prefab
```

---

**Ready to become an EDGELORD!** ⚔️
