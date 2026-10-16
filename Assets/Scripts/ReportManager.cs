using UnityEngine;

public class ReportManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AnomalyManager anomalyManager;

    [Header("Settings")]
    [SerializeField] private float reportCooldown = 3f;
    [SerializeField] private float wrongReportSanityCost = 15f;

    private float nextReportTime;

    public bool CanReport => Time.time >= nextReportTime;

    public bool SubmitReport(string room, AnomalyType type)
    {
        if (!CanReport)
        {
            Debug.Log("Please wait before reporting again.");
            return false;
        }

        bool found = anomalyManager.TryClearMatchingAnomaly(
            room,
            type,
            out bool reportedOnTime
        );

        if (found)
        {
            Debug.Log("Correct report submitted!");

            if (GameManager.Instance != null)
                GameManager.Instance.RewardCorrectReport();

            return true;
        }

        Debug.Log("Incorrect report!");
        nextReportTime = Time.time + reportCooldown;

        if (GameManager.Instance != null)
            GameManager.Instance.ReduceSanity(wrongReportSanityCost);

        return false;
    }
}