# 🏪 Tindahan

[![Platform: PC](https://img.shields.io/badge/Platform-PC-blue.svg)](https://unity.com/)
[![Engine: Unity URP](https://img.shields.io/badge/Engine-Unity_URP-lightgrey.svg)](https://unity.com/)
[![Genre: Psychological Horror / Sim](https://img.shields.io/badge/Genre-Psychological_Horror_/_Simulation-red.svg)]()

> A short-form, low-poly psychological horror game set in a familiar, atmospheric late-night Philippine residential neighborhood (*barangay*).

---

## 📖 Project Overview

**Tindahan** takes a mundane, deeply relatable cultural chore and strips away its comfort, transforming everyday streets into a claustrophobic, survival-driven nightmare.

* **The Premise:** You are hanging around a local *sari-sari* store at night when a friend calls your phone, inviting you over for an *inuman* (drinking session). The catch? You lack the required *ambag* (monetary contribution). To raise the cash quickly, you agree to complete a series of late-night delivery errands for the store owner.
* **The Twist:** As midnight approaches and the fog rolls over the streets, you discover you are not the only one stalking the unlit paths.
* **Scope:** A tightly focused 5-to-10 minute solo-developed Minimum Viable Product (MVP).

### 🛠️ Core Design Pillars
1. **Cultural Familiarity as Vulnerability:** Using nostalgic Filipino imagery to build a false sense of security before introducing psychological distortions.
2. **Mechanical Sluggishness:** Forcing heavy, deliberate movement physics rather than hyper-fast evasion to simulate human frailty and physical panic.
3. **Retro Discomfort:** Leveraging low-fidelity visual and audio systems to evoke a grainy, "found-footage" sense of grime.

---

## 🎮 Gameplay Loop & Level Layout

### The Errands Mechanics
1. **Interaction:** Receive a specific order (e.g., vinegar, ice, or cigarettes) from the *Tindahan* window.
2. **Navigation:** Explore and find the buyer of the item you are currently holding.
3. **Delivery:** Locate the correct recipient house trigger boundary, flash your light, and complete the transaction.
4. **Win Condition:** Earn a total of **₱50.00** Coins and get back to the Tindahan to safely trigger the ending.

### 🖼️ Gameplay Environment Showcase

| The Hub: The Sari-Sari Store | Night Environment Exploration |
|:---:|:---:|
| <img width="1188" height="667" alt="image" src="https://github.com/user-attachments/assets/234e32ee-0fd7-493d-b204-aed9f68dcc2e" />| <img width="1188" height="667" alt="image" src="https://github.com/user-attachments/assets/4864b5fd-1b7f-42b4-adc3-497ba6420a17" />|
| *The only bright spot on the map, acting as your safe zone.* | *Dark paths where visibility drops and scares are frequent.* |

---

## 💻 Key Implementations & Scripts

### 1. The Threat Engine: Multi-Stage Stalker AI (`ChasingAI.cs`)
The antagonist ("The Crawler") stalks the player using a three-phase execution thread mapped to Unity NavMesh pathfinding:

* **Phase 1: The Spawn & Intro Pull (3s Delay):** Temporarily overrides player input and forces the camera view to lock onto the monster (`ExecuteForcedLookAt()`). Concurrently, an automated translation vector (`ExecuteIntroPull()`) slowly drags the player forward toward the monster to simulate being frozen in fear.
* **Phase 2: The NavMesh Chase:** Player controls return. To prevent visual animation glitches (like a 3D model sliding smoothly across the floor), root transform positions and rotations are explicitly baked into their original coordinates while the `NavMeshAgent` manages translation.
* **Phase 3: The Kill State:** When the proximity falls within a defined threshold (`stopDistance <= 3.5`), locomotion freezes, a high-frequency camera shake triggers, and control passes to the Game Over panel.

| The Chase Cut Scene |
|:---:|
|<img width="1190" height="669" alt="image" src="https://github.com/user-attachments/assets/dc3b7523-1553-49cf-95eb-c6a36a53c570" />|
### 2. Optimization Layer: Performance Safeguards (`ScareTrigger.cs`)
Environmental jumpscare triggers placed throughout the map use a boolean state gate (`hasTriggered`). Once a player crosses the bounding volume, the scare elements fire and the flag instantly hard-locks to `true`. This prevents multiple execution calls, preserving memory overhead and maintaining shock value.

---

## 🎨 Art & Audio Pipeline

### The PS1 "Retro-Horror" Post-Process Shader
Instead of rendering the scene at native resolutions, technical systems were styled intentionally around retro limits:
* **Viewport Downscaling:** The main camera view renders directly into a low-resolution to emulate authentic PS1-era pixelation.
* **Pixel-Perfect Canvas UI:** To prevent high-definition text overlays from breaking immersion, the UI Canvas Render Mode is set to **Screen Space - Camera** mapped directly to the downscaled camera viewport. This enforces a uniform, jagged pixel filter across the *Barya* counter and item trackers.
* **Atmospheric Overrides:** A custom Universal Render Pipeline (URP) Global Volume profile applies real-time high-intensity shadows, low ambient environmental light, and an intense **Vignette** to enforce a tunnel-vision effect.

### 🖼️ Blender Model Showcase

| Core Hub: Sari-Sari Store Model | Custom Kapitbahays Model |
|:---:|:---:|
|<img width="1579" height="1013" alt="image" src="https://github.com/user-attachments/assets/44ea985f-67a0-4269-9b12-af1aab30b87c" />|<img width="1577" height="940" alt="image" src="https://github.com/user-attachments/assets/7a24bcc4-f464-4bed-af61-b02a0ef4cc1a" />|


### Audio Architecture
* **Ambient Drone:** Continuous background loop playing scary ambient music to build consistent tension.
* **Spike Triggers:** Localized 3D spatial audio nodes programmed to fire sudden jumpscare audio spikes featuring terrifying screams and scary chimes.

---
