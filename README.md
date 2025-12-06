# EdgelordSamuraiXR
Where Fruit Ninja meets real-world physics in a high-fidelity Mixed Reality dojo. a Mixed Reality (MR) combat and rhythm experience designed for the Meta Quest 3. It bridges the gap between digital gaming and physical athletics by utilizing a custom-mounted Katana controller interface.

The project utilizes the Meta Presence Platform in modes: one that turns real-world thrown objects into virtual targets using advanced Computer Vision, and a co-located multiplayer mode that gamifies synchronized movement and martial arts flow.

**Key Features**

**1. The "Fruit Pitch" (Real-Time Object Virtualization) An industry-first MR social feature where a physical player interacts with the VR user.** 

**How it works:** A friend throws a real ball at the VR player. Using passthrough API and custom Computer Vision (PCA & Edge Detection), the system tracks the physical ball, predicts its trajectory using Kalman Filtering to eliminate latency, and overlays it with a 3D target (e.g., a glowing fruit or cyber-orb).

**The Goal:** The VR player must slice the "digital skin" off the physical ball in mid-air, combining real-world reflexes with digital gamification.

**2. Synchro-Combat (Co-Located Multiplayer) A shared-space multiplayer experience for two players in the same physical room.**

**How it works:** Utilizing Meta’s Shared Spatial Anchors, two players align perfectly in a shared digital dojo.

**The Goal:** Players face a common virtual enemy or rhythm track, requiring them to attack, dodge, and flow in perfect unison. Scoring is based on "Resonance"—the temporal synchronization of their movements to the beat and to each other.

**Under the Hood**

**Hardware:** Meta Quest 3 with custom 3D-printed Katana Hilt mounts.

**Core Tech:** Unity 6, Meta XR All-in-One SDK (Passthrough & Depth).

**Tracking:** Custom OpenCV pipeline for blob tracking and trajectory prediction.

**Multiplayer:** Photon Fusion for tick-accurate network synchronization.
