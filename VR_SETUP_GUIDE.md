# Hello World VR - Setup Guide for Meta Quest 3

## What Has Been Set Up

### 1. Packages Installed
- Meta XR SDK All (v81.0.0) - Official Meta Quest SDK
- Unity XR Interaction Toolkit (v3.3.0) - VR interactions framework
- Unity Input System (v1.16.0) - Modern input handling
- URP (Universal Render Pipeline) (v17.3.0) - Optimized rendering

### 2. Project Configuration
- **Product Name**: Hello World VR
- **Package Name**: com.CaitlynTheAmazing.HelloWorldVR
- **Android Min SDK**: API Level 29 (Android 10)
- **Android Target SDK**: API Level 33 (Android 13)
- **Target Architectures**: ARM64 + ARMv7

### 3. Created Files
- **HelloWorldVR.cs** - Script that displays rotating "Hello World VR!" text
- **HelloWorldVR.unity** - Scene with XR Origin and Hello World text

## Next Steps to Complete Setup

### Step 1: Configure XR Plugin Management (In Unity Editor)
1. Open Unity and load the project
2. Go to **Edit > Project Settings**
3. Select **XR Plug-in Management**
4. Click the **Android tab** (Android icon)
5. Check the box for **Oculus** (Meta XR)
6. In the **Oculus** section below, ensure:
   - Stereo Rendering Mode: Multiview
   - Low Overhead Mode: Enabled

### Step 2: Install Android Build Support
If you haven't already:
1. Open **Unity Hub**
2. Click the settings icon next to your Unity version
3. Select **Add Modules**
4. Check:
   - Android Build Support
   - Android SDK & NDK Tools
   - OpenJDK
5. Click **Install**

### Step 3: Set Build Settings
1. In Unity, go to **File > Build Settings**
2. Select **Android** platform
3. Click **Switch Platform** (if not already on Android)
4. Click **Add Open Scenes** to add the HelloWorldVR scene
5. Ensure HelloWorldVR scene is checked in the build list

### Step 4: Configure Android SDK/NDK Paths
1. Go to **Edit > Preferences** (Windows) or **Unity > Preferences** (Mac)
2. Select **External Tools**
3. Make sure Android SDK, NDK, and JDK paths are set correctly
   - Unity usually auto-detects these if you installed via Unity Hub

### Step 5: Enable Developer Mode on Quest 3
1. Put on your Quest 3 headset
2. Open **Settings > System > Developer**
3. Enable **Developer Mode** (requires Meta account with developer registration)
4. To register as developer:
   - Go to https://developer.oculus.com/
   - Create/log into your Meta developer account
   - Accept developer terms

### Step 6: Connect Quest 3 and Build
1. Connect Quest 3 to your PC via USB-C cable
2. Put on the headset - you'll see a prompt to "Allow USB Debugging"
3. Click **Always allow from this computer** and then **OK**
4. In Unity, go to **File > Build Settings**
5. Make sure your Quest 3 appears in the **Run Device** dropdown
6. Click **Build And Run**
7. Choose a location to save the APK (e.g., "Builds" folder)
8. Unity will build and automatically install to your Quest 3

### Step 7: Testing
Once the build completes:
1. The app should launch automatically on your Quest 3
2. You should see "Hello World VR!" text floating in front of you
3. The text will slowly rotate
4. You can look around using your head movements

## Alternative: Manual APK Installation
If Build And Run doesn't work:
1. Build the APK using **File > Build Settings > Build**
2. Install manually using ADB:
   ```
   adb install -r path/to/your/HelloWorldVR.apk
   ```
3. Or use SideQuest: https://sidequestvr.com/

## Troubleshooting

### "Oculus not found in XR Plug-in Management"
- The Meta XR SDK should automatically register the Oculus plugin
- Try reopening Unity or reimporting the Meta XR package

### "No Android device found"
- Make sure USB debugging is enabled on Quest 3
- Try a different USB cable (must support data transfer)
- Check Windows Device Manager to see if Quest 3 is recognized
- Install Oculus ADB Drivers: https://developer.oculus.com/downloads/package/oculus-adb-drivers/

### Build Errors
- Make sure all Android paths are configured in External Tools
- Try switching platform to Android again
- Clear the Library folder and let Unity reimport

### App crashes on launch
- Check that XR Plugin Management has Oculus enabled for Android
- Verify minimum API level is set to 29 or higher
- Check Unity console for error messages

## What's Next?

### Add Hand Tracking
1. Add XR Hand components to the XR Origin
2. Configure hand tracking in Meta XR settings

### Add Controller Support
1. Add XR Controller (Action-based) components
2. Configure input actions for grab, trigger, etc.

### Add Interactable Objects
1. Create 3D objects (cubes, spheres)
2. Add XR Grab Interactable components
3. Add Rigidbody components for physics

### Explore Meta XR Samples
- The Meta XR SDK includes sample scenes with:
  - Hand tracking examples
  - Controller interaction samples
  - UI interaction templates
  - Locomotion systems

Have fun building in VR!
