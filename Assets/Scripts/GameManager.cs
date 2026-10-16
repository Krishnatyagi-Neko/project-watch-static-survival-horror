using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
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

    [Header("Rest Settings")]
    [SerializeField] private int restScoreCost = 150;
    [SerializeField] private float sanityRestored = 30f;

    [Header("Game State")]
    [SerializeField] private bool gameStarted = true;

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    public int Score => score;
    public int TargetScore => targetScore;

    public float Sanity => sanity;
    public float MaxSanity => maxSanity;

    public int RestScoreCost => restScoreCost;

    public bool IsGameOver { get; private set; }
    public bool HasWon { get; private set; }

    private float survivalScoreTimer;

    private void Awake()
    {
        // Make sure there is only one GameManager.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        maxSanity = Mathf.Max(1f, maxSanity);
        sanity = Mathf.Clamp(sanity, 0f, maxSanity);

        // Hide the Game Over screen when the scene starts.
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    private void Update()
    {
        // Stop scoring when the game is inactive or finished.
        if (!gameStarted || IsGameOver || HasWon)
            return;

        survivalScoreTimer += Time.deltaTime;

        if (survivalScoreTimer >= 1f)
        {
            survivalScoreTimer -= 1f;
            AddScore(survivalScorePerSecond);
        }
    }

    public void AddScore(int amount)
    {
        if (IsGameOver || HasWon || amount <= 0)
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
        if (IsGameOver || HasWon || amount <= 0f)
            return;

        sanity = Mathf.Max(0f, sanity - amount);

        Debug.Log("Sanity: " + sanity + " / " + maxSanity);

        if (sanity <= 0f)
            GameOver();
    }

    public void PenalizeMissedAnomaly()
    {
        ReduceSanity(sanityLossPerMissedAnomaly);
    }


    public void TryRest()
    {
        if (IsGameOver || HasWon || !gameStarted)
            return;

        if (score < restScoreCost)
        {
            Debug.Log("Not enough score to rest!");
            return;
        }

        if (sanity >= maxSanity)
        {
            Debug.Log("Sanity is already full!");
            return;
        }

        score -= restScoreCost;
        sanity = Mathf.Min(maxSanity, sanity + sanityRestored);

        Debug.Log("Rested! Sanity: " + sanity + " / " + maxSanity);
    }

    public bool CanRest()
    {
        return !IsGameOver &&
               !HasWon &&
               gameStarted &&
               score >= restScoreCost &&
               sanity < maxSanity;
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

        // Display the player's final score.
        if (finalScoreText != null)
        {
            finalScoreText.text = "FINAL SCORE: " + score;
        }

        // Show the Game Over panel.
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Game Over Panel is not assigned!");
        }

        // Stop gameplay, timers and physics.
        Time.timeScale = 0f;

        Debug.Log("GAME OVER! Sanity depleted.");
    }

    // Connect this method to the TRY AGAIN button.
    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    // Connect this method to the MAIN MENU button.
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void SetGameStarted(bool started)
    {
        if (IsGameOver || HasWon)
            return;

        gameStarted = started;
    }
}