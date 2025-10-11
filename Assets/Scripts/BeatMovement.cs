using UnityEngine;

public class BeatMovement : MonoBehaviour
{
    public float speed = 2f;

    void Update()
    {
        // Move the beat toward the camera each frame
        transform.Translate(Vector3.back * speed * Time.deltaTime);
    }
}
