using UnityEngine;

public class ReportUI : MonoBehaviour
{
    [SerializeField] private ReportManager reportManager;
    [SerializeField] private CameraManager cameraManager;

    // Hook each report button's OnClick to this and pass the type index:
    // 0 = ObjectMissing, 1 = ExtraObject, 2 = ObjectChanged, 3 = Intruder
    public void Report(int typeIndex)
    {
        string room = cameraManager.CurrentRoom;

        if (string.IsNullOrEmpty(room))
        {
            Debug.Log("Open a camera before reporting.");
            return;
        }

        reportManager.SubmitReport(room, (AnomalyType)typeIndex);
    }
}