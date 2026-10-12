
using UnityEngine;

// The types of anomalies available in the game.
public enum AnomalyType
{
    ObjectMissing,
    ExtraObject,
    ObjectChanged,
    Intruder
}

public class AnomalyController : MonoBehaviour
{
    [Header("Anomaly Information")]
    [SerializeField] private string roomName;
    [SerializeField] private AnomalyType anomalyType;

    [Header("Visual States")]
    [SerializeField] private GameObject normalState;
    [SerializeField] private GameObject anomalyState;

    // Other scripts can read these values.
    public string RoomName => roomName;
    public AnomalyType Type => anomalyType;
    public bool IsActive { get; private set; }

    private void Awake()
    {
        // Every anomaly starts in its normal state.
        SetActiveState(false);
    }

    public void Activate()
    {
        if (IsActive)
            return;

        SetActiveState(true);

        Debug.Log("Anomaly activated in " + roomName);
    }

    public void Clear()
    {
        if (!IsActive)
            return;

        SetActiveState(false);

        Debug.Log("Anomaly cleared in " + roomName);
    }

    private void SetActiveState(bool active)
    {
        IsActive = active;

        if (normalState != null)
            normalState.SetActive(!active);

        if (anomalyState != null)
            anomalyState.SetActive(active);
    }
}
