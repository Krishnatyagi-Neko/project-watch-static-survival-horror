using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform[] cameraViews;

    [Header("CCTV view")]
    [SerializeField] private CCTVOverlay overlay;          // CCTVOverlayPanel
    [SerializeField] private GameObject cctvViewVolume;    // CCTV_ViewVolume

    [Header("Switch static")]
    [SerializeField] private GameObject staticFlash;
    [SerializeField] private float flashTime = 0.2f;

    private Vector3 deskPos;
    private Quaternion deskRot;
    private Coroutine routine;

    private void Start()
    {
        deskPos = mainCamera.transform.position;
        deskRot = mainCamera.transform.rotation;
        SetCamView(false);
    }

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

        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(SwitchRoutine(index));
    }

    public void ExitToDesk()
    {
        mainCamera.transform.SetPositionAndRotation(deskPos, deskRot);
        SetCamView(false);
    }

    private IEnumerator SwitchRoutine(int index)
    {
        if (staticFlash != null) staticFlash.SetActive(true);

        mainCamera.transform.SetPositionAndRotation(
            cameraViews[index].position,
            cameraViews[index].rotation
        );
        SetCamView(true, cameraViews[index].name);

        yield return new WaitForSecondsRealtime(flashTime);

        if (staticFlash != null) staticFlash.SetActive(false);
    }

    private void SetCamView(bool on, string camName = "")
    {
        if (overlay != null)
        {
            overlay.gameObject.SetActive(on);
            if (on) overlay.SetCamName(camName);
        }
        if (cctvViewVolume != null) cctvViewVolume.SetActive(on);
    }
}