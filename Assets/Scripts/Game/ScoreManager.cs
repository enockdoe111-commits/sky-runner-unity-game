// ScoreManager.cs - Score and high-score tracking

using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;
    public static ScoreManager Instance => instance;

    private int score = 0;
    private int coins = 0;
    private float distance = 0f;
    private int highScore = 0;
    private int totalCoinsCollected = 0;
    private float bestDistance = 0f;

    public int Score => score;
    public int Coins => coins;
    public float Distance => distance;
    public int HighScore => highScore;
    public int TotalCoins => totalCoinsCollected;
    public float BestDistance => bestDistance;

    private PlayerController playerController;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        LoadHighScores();
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameActive && playerController != null)
        {
            // Update distance based on player position
            distance = playerController.transform.position.z;
            score = Mathf.FloorToInt(distance) * GameConstants.DISTANCE_POINTS_MULTIPLIER;
        }
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        score += amount * GameConstants.COIN_POINTS;
        totalCoinsCollected += amount;
    }

    public void SaveHighScores()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        if (distance > bestDistance)
        {
            bestDistance = distance;
            PlayerPrefs.SetFloat("BestDistance", bestDistance);
        }

        totalCoinsCollected += coins;
        PlayerPrefs.SetInt("TotalCoins", totalCoinsCollected);
        PlayerPrefs.Save();
    }

    private void LoadHighScores()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        bestDistance = PlayerPrefs.GetFloat("BestDistance", 0f);
        totalCoinsCollected = PlayerPrefs.GetInt("TotalCoins", 0);
    }

    public void ResetGameSession()
    {
        score = 0;
        coins = 0;
        distance = 0f;
    }
}
