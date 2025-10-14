using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip musicClip;
    [Range(60, 200)] public float BPM = 120f;

    public float SecondsPerBeat { get; private set; }
    public float TotalBeats { get; private set; }

    private double songStartDspTime;
    private bool songEnded = false;

    public static event Action OnMusicEnded;

    void Start()
    {
        if (musicClip == null)
        {
            Debug.LogError("No music clip assigned to AudioManager!");
            return;
        }

        SecondsPerBeat = 60f / BPM;
        TotalBeats = Mathf.RoundToInt(musicClip.length / SecondsPerBeat);

        songStartDspTime = AudioSettings.dspTime;
        audioSource.clip = musicClip;
        audioSource.PlayScheduled(songStartDspTime);

        Debug.Log($"Song started. BPM = {BPM}, Total beats ≈ {TotalBeats}");
    }

    void Update()
    {
        if (songEnded || audioSource == null) return;

        double elapsed = AudioSettings.dspTime - songStartDspTime;
        if (elapsed >= musicClip.length - 0.05f)
        {
            songEnded = true;
            Debug.Log("Music finished — notifying listeners.");
            OnMusicEnded?.Invoke();
        }
    }

    public float GetAccurateSongTime()
    {
        return (float)(AudioSettings.dspTime - songStartDspTime);
    }
}
