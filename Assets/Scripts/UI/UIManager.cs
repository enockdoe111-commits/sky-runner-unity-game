// UIManager.cs - Manages all UI elements and updates

using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Gameplay UI")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text coinsText;
    [SerializeField] private Text distanceText;
    [SerializeField] private Button pauseButton;

    [Header("Menus")]
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverScreen;

    [Header("Game Over UI")]
    [SerializeField] private Text finalScoreText;
    [SerializeField] private Text finalCoinsText;
    [SerializeField] private Text finalDistanceText;
    [SerializeField] private Text highScoreText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;

    [Header("Pause Menu UI")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button pauseMenuButton;

    private ScoreManager scoreManager;
    private GameManager gameManager;
    private AudioManager audioManager;

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        gameManager = FindObjectOfType<GameManager>();
        audioManager = FindObjectOfType<AudioManager>();

        // Setup button listeners
        if (pauseButton != null)
            pauseButton.onClick.AddListener(OnPauseClicked);
        if (resumeButton != null)
            resumeButton.onClick.AddListener(OnResumeClicked);
        if (restartButton != null)
            restartButton.onClick.AddListener(OnRestartClicked);
        if (menuButton != null)
            menuButton.onClick.AddListener(OnMenuClicked);
        if (pauseMenuButton != null)
            pauseMenuButton.onClick.AddListener(OnMenuClicked);

        // Show gameplay UI initially
        ShowGameplayUI();
    }

    private void Update()
    {
        if (gameManager.IsGameActive)
        {
            UpdateGameplayUI();
        }

        // Check for pause input
        InputManager inputManager = FindObjectOfType<InputManager>();
        if (inputManager != null && inputManager.GetPauseInput())
        {
            gameManager.PauseGame();
        }
    }

    private void UpdateGameplayUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {scoreManager.Score}";
        if (coinsText != null)
            coinsText.text = $"Coins: {scoreManager.Coins}";
        if (distanceText != null)
            distanceText.text = $"Distance: {scoreManager.Distance:F1}m";
    }

    public void ShowGameplayUI()
    {
        if (gameplayUI != null) gameplayUI.SetActive(true);
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (gameOverScreen != null) gameOverScreen.SetActive(false);
    }

    public void ShowPauseMenu()
    {
        if (gameplayUI != null) gameplayUI.SetActive(false);
        if (pauseMenu != null) pauseMenu.SetActive(true);
        if (gameOverScreen != null) gameOverScreen.SetActive(false);
        audioManager.PlaySFX("button");
    }

    public void ShowGameOverScreen()
    {
        scoreManager.SaveHighScores();

        if (gameplayUI != null) gameplayUI.SetActive(false);
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (gameOverScreen != null) gameOverScreen.SetActive(true);

        if (finalScoreText != null)
            finalScoreText.text = $"Score: {scoreManager.Score}";
        if (finalCoinsText != null)
            finalCoinsText.text = $"Coins Collected: {scoreManager.Coins}";
        if (finalDistanceText != null)
            finalDistanceText.text = $"Distance: {scoreManager.Distance:F1}m";
        if (highScoreText != null)
            highScoreText.text = $"High Score: {scoreManager.HighScore}";
    }

    private void OnPauseClicked()
    {
        gameManager.PauseGame();
    }

    private void OnResumeClicked()
    {
        gameManager.PauseGame();
    }

    private void OnRestartClicked()
    {
        audioManager.PlaySFX("button");
        gameManager.RestartGame();
    }

    private void OnMenuClicked()
    {
        audioManager.PlaySFX("button");
        gameManager.LoadMainMenu();
    }
}
