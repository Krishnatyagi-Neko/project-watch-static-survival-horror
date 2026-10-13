
using System.Collections.Generic;
using UnityEngine;

public class AnomalyManager : MonoBehaviour
{
    [Header("Available Anomalies")]
    [SerializeField] private List<AnomalyController> anomalies = new();

    public int ActiveCount { get; private set; }

    public bool TrySpawnAnomaly()
    {
        // Build a list containing only inactive anomalies.
        List<AnomalyController> available = new();

        foreach (AnomalyController anomaly in anomalies)
        {
            if (anomaly != null && !anomaly.IsActive)
            {
                available.Add(anomaly);
            }
        }

        // No inactive anomalies remain.
        if (available.Count == 0)
        {
            Debug.Log("No available anomalies!");
            return false;
        }

        // Select one random anomaly.
        int randomIndex = Random.Range(0, available.Count);
        AnomalyController chosen = available[randomIndex];

        // Activate the selected anomaly.
        chosen.Activate();

        // Update our active anomaly count.
        ActiveCount++;

        Debug.Log("Active anomalies: " + ActiveCount);

        return true;
    }

    public bool ClearAnomaly(AnomalyController anomaly)
    {
        if (anomaly == null || !anomaly.IsActive)
            return false;

        anomaly.Clear();
        ActiveCount--;

        Debug.Log("Active anomalies: " + ActiveCount);

        return true;
    }

    
    public bool ClearMatchingAnomaly(string room, AnomalyType type)
    {
        foreach (AnomalyController anomaly in anomalies)
        {
            if (anomaly == null || !anomaly.IsActive)
                continue;

            if (anomaly.RoomName == room && anomaly.Type == type)
            {
                return ClearAnomaly(anomaly);
            }
        }

        return false;
    }

    
    public bool TryClearMatchingAnomaly(
        string room,
        AnomalyType type,
        out bool reportedOnTime)
    {
        reportedOnTime = false;

        foreach (AnomalyController anomaly in anomalies)
        {
            if (anomaly == null || !anomaly.IsActive)
                continue;

            if (anomaly.RoomName == room && anomaly.Type == type)
            {
                // Check the deadline before clearing the anomaly.
                reportedOnTime = !anomaly.DeadlineExpired;

                // Clear it and update the active count.
                return ClearAnomaly(anomaly);
            }
        }

        return false;
    }



}
