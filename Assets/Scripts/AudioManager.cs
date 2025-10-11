using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip musicClip;
    [Range(60, 200)] public float BPM = 120f;

    public float SecondsPerBeat { get; private set; }

    private double songStartDspTime;  // ✅ DSP time reference (sample-accurate)

    void Start()
    {
        if (musicClip == null)
        {
            Debug.LogError("❌ No music clip assigned to AudioManager!");
            return;
        }

        SecondsPerBeat = 60f / BPM;

        // ✅ Schedule playback using DSP time
        songStartDspTime = AudioSettings.dspTime;
        audioSource.clip = musicClip;
        audioSource.PlayScheduled(songStartDspTime);

        Debug.Log($"🎵 Song started — one beat every {SecondsPerBeat:F2} seconds.");
    }

    // ✅ Use sample-accurate audio clock instead of Time.time
    public float GetAccurateSongTime()
    {
        return (float)(AudioSettings.dspTime - songStartDspTime);
    }
}
