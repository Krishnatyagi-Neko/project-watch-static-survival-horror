
using UnityEngine;
using UnityEngine.InputSystem;

public class AnomalyDebugTester : MonoBehaviour
{
    [SerializeField] private AnomalyManager anomalyManager;

    private void Update()
    {
        // Press N to activate a random anomaly.
        if (Keyboard.current != null &&
            Keyboard.current.nKey.wasPressedThisFrame)
        {
            if (anomalyManager != null)
            {
                anomalyManager.TrySpawnAnomaly();
            }
        }
    }
}
