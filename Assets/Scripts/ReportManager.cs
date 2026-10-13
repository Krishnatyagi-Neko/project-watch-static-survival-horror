
using UnityEngine;

public class ReportManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AnomalyManager anomalyManager;

    [Header("Settings")]
    [SerializeField] private float reportCooldown = 3f;

    private float nextReportTime;

    public bool CanReport => Time.time >= nextReportTime;

    public bool SubmitReport(string room, AnomalyType type)
    {
        // Prevent reports during the cooldown.
        if (!CanReport)
        {
            Debug.Log("Please wait before reporting again.");
            return false;
        }

        // Ask the anomaly manager to find and clear a match.
        bool correct = anomalyManager.ClearMatchingAnomaly(room, type);

        if (correct)
        {
            Debug.Log("Correct report!");
        }
        else
        {
            Debug.Log("Incorrect report!");
            nextReportTime = Time.time + reportCooldown;
        }

        return correct;
    }
}
