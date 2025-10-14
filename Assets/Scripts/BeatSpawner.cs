using UnityEngine;
using System.Collections;

public class BeatSpawner : MonoBehaviour
{
    [Header("References")]
    public AudioSource musicSource;        
    public GameObject beatPrefab;         
    public Transform spawnPoint;           // Assign SpawnPoint
    public Transform targetPoint;          // Assign Main Camera 

    [Header("Beat Settings")]
    public float spawnInterval = 0.5f;     // For 120 BPM
    public float beatSpeed = 5f;           // Units per second (movement speed)

    private bool gameOverTriggered = false;

    void Start()
    {
        StartCoroutine(SpawnBeats());
    }

    IEnumerator SpawnBeats()
    {
        float travelTime = Vector3.Distance(spawnPoint.position, targetPoint.position) / beatSpeed;

        // Pre-spawn beats before music starts
        float elapsed = 0f;
        while (elapsed < travelTime)
        {
            SpawnBeat();
            yield return new WaitForSeconds(spawnInterval);
            elapsed += spawnInterval;
        }

        // Start music playback
        musicSource.Play();

        // Spawn beats while music is still playing
        while (musicSource.isPlaying)
        {
            float remaining = musicSource.clip.length - musicSource.time;
            if (remaining <= spawnInterval * 1.1f)
                break; // stop spawning early so the last beat aligns properly

            SpawnBeat();
            yield return new WaitForSeconds(spawnInterval);
        }

        // Wait until all beats are destroyed
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Beat").Length == 0);

        // Wait for a short cinematic pause
        yield return new WaitForSeconds(4f);

        TriggerGameOver();
    }

    void SpawnBeat()
    {
        Instantiate(beatPrefab, spawnPoint.position, Quaternion.identity);
    }

    void TriggerGameOver()
    {
        if (gameOverTriggered) return;
        gameOverTriggered = true;

        
        Debug.Log("Game Over Triggered — Final Score Displayed!");
    }
}
