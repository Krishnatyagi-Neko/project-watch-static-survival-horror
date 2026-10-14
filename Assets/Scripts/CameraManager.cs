
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform[] cameraViews;

    public void SwitchCamera(int index)
    {
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera is not assigned!");
            return;
        }

        if (cameraViews == null ||
            index < 0 ||
            index >= cameraViews.Length ||
            cameraViews[index] == null)
        {
            Debug.LogError("Invalid camera view index: " + index);
            return;
        }

        mainCamera.transform.SetPositionAndRotation(
            cameraViews[index].position,
            cameraViews[index].rotation
        );

        Debug.Log("Switched to camera view: " + cameraViews[index].name);
    }
}
