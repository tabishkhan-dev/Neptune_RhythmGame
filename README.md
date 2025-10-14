# Neptune Rhythm Game

A minimal rhythm-based Unity game built as part of the Neptune Challenge.  
Tap or click in sync with the beats of the background track to score **Perfect**, **Good**, or **Miss** hits!

---

## Challenge Completed
Implemented beat-synchronized tapping gameplay.  
Audio-driven beat spawning and scoring logic.  
On-screen feedback using TextMeshPro.  
Scoring summary display at game end.

---

## Unity Project Details
| Parameter | Value |
|------------|--------|
| **Unity Version** | 6000.0.47f1 (Unity 6 LTS) |
| **Render Pipeline** | Universal Render Pipeline (URP) |
| **Platform Tested** | Windows PC |
| **Packages Used** | TextMeshPro, URP Core |

---

## Architecture Overview

The project follows a **modular architecture** with separated responsibilities:

| Script | Description |

| `AudioManager.cs` | Handles audio playback, BPM configuration, and accurate song timing using DSP clock. |
| `BeatSpawner.cs` | Spawns beats at each calculated interval (`SecondsPerBeat`). |
| `BeatMovement.cs` | Moves beats visually toward the player for rhythm feedback. |
| `InputHandler.cs` | Detects player clicks and evaluates timing accuracy. |
| `FeedbackUI.cs` | Displays feedback text (Perfect, Good, Miss) and final score summary. |

---

## Gameplay Summary
- Click or tap **in rhythm** with the song’s beats.  
- Timing is judged as:
  - **Perfect:** ±100 ms  
  -  **Good:** ±220 ms  
  - **Miss:** >220 ms  
- At the end of the track, a **score summary** appears showing performance.

---

##  How to Run
1. Open the project in **Unity 6 (6000.0.47f1)**.  
2. Load the scene: `Assets/Scenes/SampleScene.unity`.  
3. Press **Play** in the Editor.  
4. Tap/click to the beat!  

##  Notes & Improvements
- The BPM is fixed at **120** for simplicity.  
- Latency compensation of **–100 ms** ensures realistic PC response.  
- Possible future improvements:
  - Waveform-based beat detection.  
  - Visual hit effects & combo counter.  
  - Background animations and difficulty levels.  
  - Menu system with replay/reset.

---

## Author
**Tabish Khan**  
M.Sc. Media Engineering, TU Ilmenau  
Focus: Interactive Audio & XR Systems

---

##  License
This project is for educational and evaluation purposes only.
