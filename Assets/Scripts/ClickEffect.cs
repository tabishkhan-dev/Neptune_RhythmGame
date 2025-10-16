using UnityEngine;

public class ClickEffect : MonoBehaviour
{
    public float radius = 0.1f;
    public float fadeSpeed = 2f;
    private float fade = 0f;
    private Vector3 pos;
    private GameObject tempSphere;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pos = Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1f)
            );

            if (tempSphere != null)
                Destroy(tempSphere);

            tempSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            tempSphere.transform.position = pos;
            tempSphere.transform.localScale = Vector3.one * radius;

            var mat = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
            mat.color = new Color(1f, 1f, 1f, 0.3f);
            tempSphere.GetComponent<Renderer>().material = mat;

            fade = 1f;
        }

        if (tempSphere != null)
        {
            fade -= Time.deltaTime * fadeSpeed;
            Color c = tempSphere.GetComponent<Renderer>().material.color;
            c.a = Mathf.Max(0, fade * 0.3f);
            tempSphere.GetComponent<Renderer>().material.color = c;

            if (fade <= 0f)
                Destroy(tempSphere);
        }
    }
}
