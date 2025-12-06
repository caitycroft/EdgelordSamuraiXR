# Edgelord Samurai XR - Complete Dojo Passthrough Setup Guide

## Part 1: Create New Unity Project with Meta XR SDK

### Step 1: Create New Unity Project (Unity Hub)

1. **Open Unity Hub**
2. **Click "New Project"**
3. **Select Unity Version**: 6000.2.10f1
4. **Choose Template**: "3D (URP)" - Universal Render Pipeline
5. **Project Name**: `EdgelordSamuraiXR`
6. **Location**: Choose your desired location
7. **Click "Create Project"**

Unity will take a few minutes to create and open the project.

---

### Step 2: Install Meta XR All-in-One SDK

1. **Open Package Manager**:
   - Window > Package Manager

2. **Add Meta XR SDK**:
   - Click the `+` button (top-left)
   - Select "Add package from git URL..."
   - Enter: `com.meta.xr.sdk.all`
   - Click "Add"
   - Wait for installation (2-3 minutes)

3. **Import TMP Essentials** (if prompted):
   - When TextMeshPro window appears
   - Click "Import TMP Essentials"

---

### Step 3: Configure Project for Meta Quest 3

1. **Open Project Settings**:
   - Edit > Project Settings

2. **XR Plug-in Management**:
   - Click "XR Plug-in Management" in left sidebar
   - Switch to **Android** tab (desktop icon at top)
   - Check ✓ **Oculus** (Meta XR)
   - A warning may appear - click "Fix" if needed

3. **Player Settings** (same window):
   - Click "Player" in left sidebar
   - **Android Tab**:
     - **Product Name**: "Edgelord Samurai XR"
     - **Company Name**: Your name
     - **Other Settings** section:
       - Graphics APIs: Remove "Vulkan" if present, keep only "OpenGLES3"
       - Minimum API Level: Android 10.0 (API level 29)
       - Target API Level: Android 13.0 (API level 33)
       - Scripting Backend: IL2CPP
       - Target Architectures: ✓ ARM64 (uncheck ARMv7)

4. **Quality Settings**:
   - Edit > Project Settings > Quality
   - Select "Medium" as default for Android

5. **Close Project Settings**

---

## Part 2: Set Up Passthrough Mixed Reality Scene

### Step 4: Create Main Scene

1. **Save Current Scene**:
   - File > Save As
   - Name: `DojoPassthrough`
   - Location: `Assets/Scenes/`

2. **Delete Default Objects**:
   - Delete "Main Camera" from Hierarchy
   - Delete "Directional Light" (we'll use Meta's Building Blocks)

---

### Step 5: Add Meta XR Building Blocks (Easy Setup!)

1. **Open Meta Building Blocks**:
   - Top menu: Meta > Tools > Building Blocks

2. **Add Camera Rig**:
   - In Building Blocks window, find "Camera Rig"
   - Click "Add" button
   - This creates: OVRCameraRig with left/right controllers

3. **Add Passthrough**:
   - In Building Blocks window, find "Passthrough"
   - Click "Add" button
   - This enables see-through AR mode

4. **Add Controller Tracking** (for hands):
   - Find "Controller Tracking"
   - Click "Add"

**Building Blocks window can now be closed**

---

### Step 6: Create the Katana (Right Hand)

1. **Find Right Controller**:
   - In Hierarchy, expand: OVRCameraRig > TrackingSpace > RightHandAnchor

2. **Create Katana Handle**:
   ```
   Right-click RightHandAnchor > Create Empty
   Name: "Katana"
   Position: (0, 0, 0)
   Rotation: (0, 0, 0)
   ```

3. **Create Handle Visual**:
   ```
   Right-click Katana > 3D Object > Cylinder
   Name: "Handle"
   Transform:
     Position: (0, 0, 0)
     Rotation: (0, 0, 90)
     Scale: (0.03, 0.08, 0.03)
   ```

4. **Create Handle Wrap (Tsuka-Ito)**:
   ```
   Right-click Handle > 3D Object > Cube
   Name: "HandleWrap"
   Transform:
     Position: (0, 0, 0)
     Rotation: (0, 0, 0)
     Scale: (0.035, 0.015, 0.035)
   Material: Create new, color: dark brown/black
   ```

5. **Create Guard (Tsuba)**:
   ```
   Right-click Katana > 3D Object > Cylinder
   Name: "Guard"
   Transform:
     Position: (0.09, 0, 0)
     Rotation: (0, 0, 90)
     Scale: (0.08, 0.005, 0.08)
   Material: Create new, color: metallic silver/gold
   ```

6. **Create Blade**:
   ```
   Right-click Katana > 3D Object > Cube
   Name: "Blade"
   Transform:
     Position: (0.45, 0, 0)  // Extends forward from guard
     Rotation: (0, 0, 0)
     Scale: (0.7, 0.01, 0.04)  // Long, thin, flat
   ```

7. **Style the Blade**:
   - Select Blade
   - Create Material: Assets > Create > Material
   - Name: "KatanaBlade"
   - Properties:
     - Metallic: 1
     - Smoothness: 0.9
     - Color: Silver/White with slight blue tint
   - Drag material onto Blade

8. **Add Blade Trail**:
   - Select Blade
   - Add Component > Trail Renderer
   - Settings:
     - Time: 0.3
     - Min Vertex Distance: 0.1
     - Width: Start = 0.08, End = 0
     - Materials: Create new Material
       - Shader: Particles/Standard Unlit
       - Rendering Mode: Fade
       - Color: White to transparent gradient
     - Emit: Will be controlled by script

9. **Add Blade Collider**:
   - Select Blade
   - Add Component > Box Collider
   - Is Trigger: ✓
   - Size: (0.7, 0.05, 0.08)

---

### Step 7: Add Scripts to Katana

1. **Create Scripts Folder** (if not exists):
   - In Project window: Assets > Create > Folder
   - Name: "Scripts"

2. **Copy Decart XR Scripts**:
   - Copy all files from your old project's `Assets/Scripts/DecartXR/` folder
   - Paste into new project's `Assets/Scripts/` folder

   OR if starting fresh, create them using the code from the previous setup.

3. **Add KatanaController to Katana**:
   - Select "Katana" object (parent)
   - Add Component > Katana Controller
   - Assign:
     - Controller Node: Right Hand
     - Blade Transform: Drag "Blade" object here
     - Slash Trail: Drag Trail Renderer from Blade
     - Show Debug Info: ✓ (for testing)

---

## Part 3: Create Japanese Dojo Environment

### Step 8: Set Up Dojo Structure

1. **Create Dojo Parent**:
   ```
   Hierarchy > Create Empty
   Name: "Dojo"
   Position: (0, 0, 0)
   ```

2. **Create Floor**:
   ```
   Right-click Dojo > 3D Object > Plane
   Name: "DojoFloor"
   Transform:
     Position: (0, 0, 0)
     Rotation: (0, 0, 0)
     Scale: (3, 1, 3)  // 30m x 30m floor
   ```

3. **Style Floor (Tatami Mat)**:
   - Create Material: "TatamiMat"
   - Color: Tan/beige (#D4C5A9)
   - Or use a tatami texture if you have one
   - Apply to DojoFloor

4. **Create Bamboo Screens (Shoji)**:

   **Screen 1 - Back Wall**:
   ```
   Right-click Dojo > 3D Object > Plane
   Name: "ShojiScreen_Back"
   Transform:
     Position: (0, 1.5, -3)
     Rotation: (0, 0, 0)
     Scale: (2, 1.5, 1)
   ```

   **Screen 2 - Left Wall**:
   ```
   Duplicate ShojiScreen_Back (Ctrl+D)
   Name: "ShojiScreen_Left"
   Transform:
     Position: (-3, 1.5, 0)
     Rotation: (0, 90, 0)
     Scale: (2, 1.5, 1)
   ```

   **Screen 3 - Right Wall**:
   ```
   Duplicate ShojiScreen_Left
   Name: "ShojiScreen_Right"
   Transform:
     Position: (3, 1.5, 0)
     Rotation: (0, -90, 0)
     Scale: (2, 1.5, 1)
   ```

5. **Style Bamboo Screens**:
   - Create Material: "BambooScreen"
   - Color: Cream/off-white (#F5F5DC)
   - Rendering Mode: Transparent (for semi-transparent effect)
   - Alpha: 0.7
   - Apply to all Shoji screens

6. **Add Bamboo Frames**:
   ```
   For each screen, create a grid pattern:

   Right-click ShojiScreen_Back > 3D Object > Cube
   Name: "Frame_Vertical1"
   Transform:
     Position: (-0.5, 0, 0.01)
     Scale: (0.02, 1, 0.02)
   Material: Dark brown/black (#3D2817)

   Duplicate for vertical and horizontal frames to create a grid
   ```

---

### Step 9: Add Japanese Decorations

1. **Create Torii Gate** (Optional entrance):
   ```
   Create using cylinders for posts and cubes for crossbeams
   Position in front of player spawn area
   Material: Red/vermillion color (#E60012)
   ```

2. **Add Lanterns**:
   ```
   Right-click Dojo > 3D Object > Sphere
   Name: "Lantern"
   Scale: (0.3, 0.4, 0.3)
   Position: (2, 2, -2)

   Add Component > Light
     Type: Point Light
     Color: Warm orange
     Range: 3
     Intensity: 2

   Material: Paper lantern (white/cream)

   Duplicate for multiple lanterns around dojo
   ```

3. **Add Bamboo Plants**:
   ```
   Right-click Dojo > 3D Object > Cylinder
   Name: "Bamboo"
   Scale: (0.05, 2, 0.05)
   Material: Green (#2D5016)
   Position: In corners
   ```

---

### Step 10: Configure Passthrough Blend

1. **Select OVRCameraRig** in Hierarchy

2. **Find OVR Passthrough Layer Component**

3. **Settings**:
   - Projection Surface Type: User Defined
   - Compositing Layer: Underlay (dojo appears behind real world)
   - OR Overlay (dojo appears in front)
   - Edge Color: Can add colored edge effect
   - Opacity: 0.5-0.8 (adjust for blend effect)

4. **Passthrough Blend**:
   - You can adjust how much real-world you see vs virtual dojo
   - Lower opacity = more real world
   - Higher opacity = more virtual dojo

---

## Part 4: Create Fruit Slicing System

### Step 11: Create Fruit Prefabs

**Orange:**
```
Hierarchy > 3D Object > Sphere
Name: "Orange"
Scale: (0.15, 0.15, 0.15)

Components:
- Rigidbody (Mass: 0.3, Use Gravity: ✓)
- Sphere Collider (Is Trigger: ✓)
- SliceableObject script
  - Base Points: 100

Material:
- Color: Orange (#FF8C00)
- Smoothness: 0.6

Make Prefab:
- Drag to Assets/Prefabs/ folder
```

**Apple:**
```
Same as Orange but:
- Color: Red (#DC143C)
- Scale: (0.12, 0.13, 0.12) (slightly oval)
- Base Points: 150
```

**Watermelon:**
```
Same structure but:
- Scale: (0.25, 0.22, 0.25) (larger, oval)
- Color: Dark green (#0D5F0D) with dark green stripes
- Base Points: 200
```

**Banana:**
```
Use Capsule instead of Sphere
- Scale: (0.08, 0.15, 0.08)
- Rotation: (0, 0, 45) for curved look
- Color: Yellow (#FFE135)
- Base Points: 120
```

### Step 12: Create Fruit Spawner

1. **Create Spawner Object**:
   ```
   Hierarchy > Create Empty
   Name: "FruitSpawner"
   Position: (0, 1, 5)  // Behind player, will throw toward them
   ```

2. **Add ObjectSpawner Script**:
   - Add Component > Object Spawner
   - Settings:
     - Object Prefab: Drag Orange prefab (we'll randomize later)
     - Target Position: Drag OVRCameraRig > TrackingSpace > CenterEyeAnchor
     - Spawn Interval: 1.5 seconds
     - Spawn Variation: 0.3
     - Spawn Radius: 2 meters
     - Spawn Height Range: (1, 2.5)
     - Launch Force: 6
     - Launch Variance: 0.3
     - Object Lifetime: 8 seconds

3. **Randomize Fruit Types** (Enhanced Spawner):

   Create a new script `RandomFruitSpawner.cs`:

```csharp
using UnityEngine;

public class RandomFruitSpawner : MonoBehaviour
{
    [Header("Fruit Prefabs")]
    public GameObject[] fruitPrefabs;

    [Header("Spawn Settings")]
    public float spawnInterval = 1.5f;
    public float spawnVariation = 0.3f;
    public Transform targetPosition;
    public float launchForce = 6f;
    public Vector2 spawnHeightRange = new Vector2(1f, 2.5f);
    public float spawnRadius = 2f;

    private float nextSpawnTime;

    void Start()
    {
        if (targetPosition == null)
        {
            targetPosition = Camera.main.transform;
        }
        nextSpawnTime = Time.time + spawnInterval;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnRandomFruit();
            nextSpawnTime = Time.time + spawnInterval + Random.Range(-spawnVariation, spawnVariation);
        }
    }

    void SpawnRandomFruit()
    {
        if (fruitPrefabs.Length == 0) return;

        // Pick random fruit
        GameObject fruitPrefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];

        // Random spawn position
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float distance = Random.Range(spawnRadius * 0.5f, spawnRadius);
        float height = Random.Range(spawnHeightRange.x, spawnHeightRange.y);

        Vector3 spawnPos = transform.position + new Vector3(
            Mathf.Cos(angle) * distance,
            height,
            Mathf.Sin(angle) * distance
        );

        // Spawn fruit
        GameObject fruit = Instantiate(fruitPrefab, spawnPos, Random.rotation);

        // Launch toward player
        Rigidbody rb = fruit.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = (targetPosition.position - spawnPos).normalized;
            rb.AddForce(direction * launchForce, ForceMode.Impulse);
            rb.AddTorque(Random.insideUnitSphere * 2f, ForceMode.Impulse);
        }

        // Auto-destroy
        Destroy(fruit, 10f);
    }
}
```

4. **Replace ObjectSpawner with RandomFruitSpawner**:
   - Select FruitSpawner object
   - Remove ObjectSpawner component
   - Add Component > Random Fruit Spawner
   - Assign:
     - Fruit Prefabs: Size = 4, drag all fruit prefabs
     - Target Position: CenterEyeAnchor
     - Adjust other settings as needed

---

## Part 5: Set Up Decart XR API & UI

### Step 13: Add Decart XR Manager

```
Hierarchy > Create Empty
Name: "DecartXRManager"
Add Component > Decart XRAPI

Default settings are good, but you can tune:
- Perfect Timing Window: 0.1s
- Perfect Hasuji Angle: 15°
- Min Cut Velocity: 2 m/s
```

### Step 14: Create UI Canvas

1. **Create Canvas**:
   ```
   Hierarchy > UI > Canvas
   Name: "GameUI"
   ```

2. **Configure Canvas for VR**:
   - Render Mode: World Space
   - Position: (0, 2.5, 2)  // Above and in front of player
   - Rotation: (15, 0, 0)  // Tilted down slightly
   - Scale: (0.002, 0.002, 0.002)
   - Width: 1920
   - Height: 1080

3. **Add Background Panel** (optional):
   ```
   Right-click GameUI > UI > Panel
   Name: "Background"
   Color: Black with alpha 0.3 (semi-transparent)
   ```

4. **Add Feedback Text**:
   ```
   Right-click GameUI > UI > Text - TextMeshPro
   Name: "FeedbackText"

   Rect Transform:
     - Anchor: Center
     - Position: (0, 100, 0)
     - Width: 1000, Height: 400

   TextMeshPro Settings:
     - Font Size: 120
     - Alignment: Center/Middle
     - Color: White
     - Font Style: Bold
     - Enable Auto Size: ✓
   ```

5. **Add Score Text**:
   ```
   Right-click GameUI > UI > Text - TextMeshPro
   Name: "ScoreText"

   Rect Transform:
     - Anchor: Top-Left
     - Position: (100, -100, 0)
     - Width: 500, Height: 100

   TextMeshPro:
     - Font Size: 72
     - Text: "SCORE: 0"
     - Color: Gold (#FFD700)
     - Alignment: Left/Top
   ```

6. **Add Combo Text**:
   ```
   Right-click GameUI > UI > Text - TextMeshPro
   Name: "ComboText"

   Rect Transform:
     - Anchor: Top-Right
     - Position: (-100, -100, 0)
     - Width: 500, Height: 150

   TextMeshPro:
     - Font Size: 64
     - Color: Cyan (#00FFFF)
     - Alignment: Right/Top
   ```

7. **Add SamuraiFeedbackUI Script**:
   - Select GameUI
   - Add Component > Samurai Feedback UI
   - Assign all text references (drag from Hierarchy)

---

### Step 15: Connect Everything

1. **Update SliceableObject Prefabs**:
   - Open each fruit prefab
   - In SliceableObject component:
     - Add particle effect if you have one
     - Add slice sound if you have one
   - Save prefab

2. **Test Connections**:
   - Make sure Katana has KatanaController
   - Make sure DecartXRManager exists in scene
   - Make sure all UI texts are assigned

---

## Part 6: Build Settings & Final Polish

### Step 16: Configure Build Settings

1. **Open Build Settings**:
   - File > Build Settings

2. **Add Scene**:
   - Click "Add Open Scenes" (adds DojoPassthrough)
   - Remove any other scenes

3. **Platform**:
   - Select Android
   - Click "Switch Platform" if needed

4. **Texture Compression**:
   - Set to ASTC

### Step 17: Add Particle Effects (Optional)

1. **Fruit Slice Effect**:
   ```
   Hierarchy > Right-click > Effects > Particle System
   Name: "FruitSliceParticles"

   Main Module:
     - Duration: 0.5
     - Start Lifetime: 0.3-0.6
     - Start Speed: 2-5
     - Start Size: 0.05-0.15
     - Start Color: Orange (matches fruit)

   Emission:
     - Rate over Time: 0
     - Bursts: 1 burst at time 0, Count 20-30

   Shape:
     - Shape: Sphere
     - Radius: 0.1

   Make into Prefab
   Assign to fruit prefabs in SliceableObject component
   ```

---

## Part 7: Build and Deploy to Meta Quest 3

### Step 18: Build APK

1. **Player Settings** (File > Build Settings > Player Settings):
   - Product Name: "Edgelord Samurai XR"
   - Package Name: com.YourName.EdgelordSamuraiXR

2. **Build**:
   - File > Build Settings
   - Click "Build and Run"
   - Save as: `Builds/EdgelordSamuraiXR.apk`
   - Connect Quest 3 via USB
   - Wait for build (10-15 minutes first time)

### Step 19: Enable Passthrough on Quest

1. **Put on Quest 3**
2. **When app launches**:
   - You should see your real room with virtual dojo elements overlaid
   - Katana in your right hand
   - Fruit flying at you

3. **Adjust Passthrough**:
   - In Unity, adjust OVR Passthrough Layer opacity for desired blend

---

## Testing Checklist

- [ ] Katana appears in right hand
- [ ] Katana has visible trail when swinging
- [ ] Passthrough shows real environment
- [ ] Dojo elements (floor, screens, lanterns) visible
- [ ] Fruit spawn and fly toward player
- [ ] Fruit can be sliced with katana
- [ ] UI shows score, combo, and feedback
- [ ] Japanese text appears on good slices (絶対領域！etc.)
- [ ] Combo multiplier increases with consecutive cuts
- [ ] Sound effects play (if added)

---

## Troubleshooting

**Katana not visible:**
- Check it's child of RightHandAnchor
- Verify materials are assigned
- Check scale isn't too small

**Passthrough not working:**
- Verify Oculus XR Plugin is enabled
- Check Passthrough building block was added
- Ensure you granted camera permissions on Quest

**Fruit not slicing:**
- Verify blade has trigger collider
- Check KatanaController is detecting slashes (debug logs)
- Ensure SliceableObject has is Trigger enabled

**UI not showing:**
- Check Canvas position in front of camera
- Verify World Space render mode
- Check text components are assigned in SamuraiFeedbackUI

---

## Final Result

You now have:
- ✅ Passthrough mixed reality
- ✅ Japanese dojo environment
- ✅ Visible katana in hand
- ✅ Fruit flying at you
- ✅ Full Decart XR scoring system
- ✅ Japanese-style feedback
- ✅ Combo system

**Time to become an EDGELORD! ⚔️🍊**
