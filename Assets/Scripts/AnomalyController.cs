using UnityEngine;

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

    [Header("Report Deadline")]
    [SerializeField] private float reportTimeLimit = 15f;

    // Fired once when the report deadline runs out (AnomalyManager listens to this)
    public event System.Action<AnomalyController> DeadlineExpiredEvent;

    public string RoomName => roomName;
    public AnomalyType Type => anomalyType;
    public bool IsActive { get; private set; }

    public float RemainingReportTime { get; private set; }
    public bool DeadlineExpired { get; private set; }

    private void Awake()
    {
        SetActiveState(false);
    }

    private void Update()
    {
        // Only count down while this anomaly is active.
        if (!IsActive || DeadlineExpired)
            return;

        RemainingReportTime -= Time.deltaTime;

        // The deadline has expired.
        if (RemainingReportTime <= 0f)
        {
            RemainingReportTime = 0f;
            DeadlineExpired = true;

            Debug.Log("Report deadline expired in " + roomName);

           if (GameManager.Instance != null)
            {
                GameManager.Instance.PenalizeMissedAnomaly();
            }

            // Sanity penalty is applied first, then the manager clears the anomaly
            DeadlineExpiredEvent?.Invoke(this);
        }
    }

    public void Activate()
    {
        if (IsActive)
            return;

        RemainingReportTime = reportTimeLimit;
        DeadlineExpired = false;

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