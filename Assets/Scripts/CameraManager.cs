
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // Holds the name and viewpoint of one security camera.
    [System.Serializable]
    public class SecurityCamera
    {
        public string cameraName;
        public Transform viewpoint;
    }

    // The actual camera that displays the game.
    [SerializeField] private Camera mainCamera;

    // All available security camera viewpoints.
    [SerializeField] private SecurityCamera[] cameras;

    // Called by a UI button to switch the current view.
    public void SwitchCamera(int index)
    {
        // Prevent invalid array indexes.
        if (index < 0 || index >= cameras.Length)
            return;

        // Get the selected camera's viewpoint.
        Transform target = cameras[index].viewpoint;

        // Make sure a viewpoint has been assigned.
        if (target == null)
            return;

        // Move and rotate the Main Camera to the viewpoint.
        mainCamera.transform.SetPositionAndRotation(
            target.position,
            target.rotation
        );
    }
}
