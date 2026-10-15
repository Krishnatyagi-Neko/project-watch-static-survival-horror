using UnityEngine;

public class FloatingCup : MonoBehaviour
{
    [SerializeField] private float height = 0.5f;
    [SerializeField] private float riseDuration = 2.5f;
    [SerializeField] private float bobAmount = 0.03f;
    [SerializeField] private float spinSpeed = 40f;

    private Vector3 startPos;       // world space
    private Quaternion startRot;    // world space
    private float riseT;

    private void OnEnable()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        riseT = 0f;
    }

    private void OnDisable()
    {
        transform.SetPositionAndRotation(startPos, startRot);
    }

    private void Update()
    {
        riseT = Mathf.MoveTowards(riseT, 1f, Time.deltaTime / riseDuration);

        float lift = height * Mathf.SmoothStep(0f, 1f, riseT);
        float bob = Mathf.Sin(Time.time * 2f) * bobAmount * riseT;

        transform.position = startPos + Vector3.up * (lift + bob);
        transform.Rotate(Vector3.up, spinSpeed * riseT * Time.deltaTime, Space.World);
    }
}