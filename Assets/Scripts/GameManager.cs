
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Allows other scripts to access the GameManager.
    public static GameManager Instance { get; private set; }

    [Header("Score Settings")]
    [SerializeField] private int score = 0;
    [SerializeField] private int targetScore = 1000;
    [SerializeField] private int survivalScorePerSecond = 1;
    [SerializeField] private int correctReportBonus = 100;

    [Header("Sanity Settings")]
    [SerializeField] private float maxSanity = 100f;
    [SerializeField] private float sanity = 100f;
    [SerializeField] private float sanityLossPerMissedAnomaly = 25f;

    [Header("Game State")]
    [SerializeField] private bool gameStarted = true;

    public int Score => score;
    public int TargetScore => targetScore;
    public float Sanity => sanity;
    public float MaxSanity => maxSanity;
    public bool IsGameOver { get; private set; }
    public bool HasWon { get; private set; }

    private float survivalScoreTimer;

    private void Awake()
    {
        // Ensure there is only one GameManager.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (!gameStarted || IsGameOver || HasWon)
            return;

        // Award survival points once per second.
        survivalScoreTimer += Time.deltaTime;

        if (survivalScoreTimer >= 1f)
        {
            survivalScoreTimer -= 1f;
            AddScore(survivalScorePerSecond);
        }
    }

    public void AddScore(int amount)
    {
        if (IsGameOver || HasWon)
            return;

        score += amount;

        if (score >= targetScore)
            Victory();
    }

    public void RewardCorrectReport()
    {
        AddScore(correctReportBonus);
    }

    public void ReduceSanity(float amount)
    {
        if (IsGameOver || HasWon)
            return;

        sanity = Mathf.Max(0f, sanity - amount);

        Debug.Log("Sanity: " + sanity + " / " + maxSanity);

        if (sanity <= 0f)
            GameOver();
    }

    public void Victory()
    {
        if (IsGameOver || HasWon)
            return;

        HasWon = true;
        gameStarted = false;

        Debug.Log("VICTORY! Target score reached.");
    }

    public void GameOver()
    {
        if (IsGameOver || HasWon)
            return;

        IsGameOver = true;
        gameStarted = false;

        Debug.Log("GAME OVER! Sanity depleted.");
    }
}
