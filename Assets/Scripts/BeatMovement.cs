using UnityEngine;

public class BeatMovement : MonoBehaviour
{
    // This will be set by the BeatSpawner when the object is created.
    public float speed = 2f;

    void Update()
    {
        // Moves the beat towards the default 'back' direction (towards Z = negative)
        transform.Translate(Vector3.back * speed * Time.deltaTime);
    }
}