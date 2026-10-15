using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CCTVFrameRate : MonoBehaviour
{
    [SerializeField] float fps = 12f;
    Camera cam;
    float next;

    void Awake()
    {
        cam = GetComponent<Camera>();
        cam.enabled = false;   // we trigger renders manually
    }

    void Update()
    {
        if (Time.unscaledTime < next) return;
        next = Time.unscaledTime + 1f / fps;
        cam.Render();
    }
}