using UnityEngine;

public class BeatSpawner : MonoBehaviour
{
    [Header("References")]
    public AudioManager audioManager;   // link to your AudioManager
    public GameObject beatPrefab;       // visual object to spawn
    public Transform spawnPoint;        // where to spawn it

    private float nextBeatTime;
    private bool songEnded = false;

    void Start()
    {
        if (audioManager == null)
        {
            Debug.LogError("❌ BeatSpawner: AudioManager reference missing!");
            return;
        }

        nextBeatTime = audioManager.SecondsPerBeat;
        songEnded = false;
    }

    void Update()
    {
        if (audioManager == null)
            return;

        AudioSource src = audioManager.GetComponent<AudioSource>();

        // 🛑 Stop spawning if the song ended
        if (!songEnded && !src.isPlaying)
        {
            songEnded = true;
            Debug.Log("🎵 Song ended — stopping beat spawns.");
            return;
        }

        // If still playing, spawn on beat
        float songTime = audioManager.GetAccurateSongTime();
        if (!songEnded && songTime >= nextBeatTime)
        {
            SpawnBeat();
            nextBeatTime += audioManager.SecondsPerBeat;
        }
    }

    void SpawnBeat()
    {
        if (beatPrefab != null && spawnPoint != null)
        {
            Instantiate(beatPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}
