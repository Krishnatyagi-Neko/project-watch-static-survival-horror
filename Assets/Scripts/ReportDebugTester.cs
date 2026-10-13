
using UnityEngine;
using UnityEngine.InputSystem;

public class ReportDebugTester : MonoBehaviour
{
    [SerializeField] private ReportManager reportManager;

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        // Test report: Living Room, Object Changed.
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            reportManager.SubmitReport(
                "Living Room",
                AnomalyType.ObjectChanged
            );
        }
    }
}
