// GameManager.cs - Main game loop and state management

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState { Menu, Playing, Paused, GameOver }

    private static GameManager instance;
    public static GameManager Instance => instance;

    [HideInInspector] public GameState CurrentState { get; private set; }
    [HideInInspector] public bool IsGameActive => CurrentState == GameState.Playing;

    private float gameTime = 0f;
    private int currentDifficultyLevel = 1;
    private float currentSpeedMultiplier = 1f;

    private PlayerController playerController;
    private ScoreManager scoreManager;
    private UIManager uiManager;
    private AudioManager audioManager;
    private TrackGenerator trackGenerator;
    private ObstacleSpawner obstacleSpawner;

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
        // Initialize all managers
        playerController = FindObjectOfType<PlayerController>();
        scoreManager = FindObjectOfType<ScoreManager>();
        uiManager = FindObjectOfType<UIManager>();
        audioManager = FindObjectOfType<AudioManager>();
        trackGenerator = FindObjectOfType<TrackGenerator>();
        obstacleSpawner = FindObjectOfType<ObstacleSpawner>();

        SetGameState(GameState.Playing);
        audioManager.PlayBackgroundMusic();
    }

    private void Update()
    {
        if (CurrentState == GameState.Playing)
        {
            gameTime += Time.deltaTime;
            UpdateDifficulty();
            CheckGameOver();
        }
    }

    public void SetGameState(GameState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;

        switch (newState)
        {
            case GameState.Playing:
                Time.timeScale = 1f;
                playerController.enabled = true;
                audioManager.ResumeBackgroundMusic();
                uiManager.ShowGameplayUI();
                break;

            case GameState.Paused:
                Time.timeScale = 0f;
                playerController.enabled = false;
                audioManager.PauseBackgroundMusic();
                uiManager.ShowPauseMenu();
                break;

            case GameState.GameOver:
                Time.timeScale = 0f;
                playerController.enabled = false;
                audioManager.StopBackgroundMusic();
                audioManager.PlaySFX("gameOver");
                uiManager.ShowGameOverScreen();
                break;

            case GameState.Menu:
                Time.timeScale = 1f;
                break;
        }
    }

    public void PauseGame()
    {
        if (CurrentState == GameState.Playing)
            SetGameState(GameState.Paused);
        else if (CurrentState == GameState.Paused)
            SetGameState(GameState.Playing);
    }

    public void GameOver()
    {
        SetGameState(GameState.GameOver);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void UpdateDifficulty()
    {
        int newLevel = (scoreManager.Score / GameConstants.SCORE_FOR_DIFFICULTY_INCREASE) + 1;
        if (newLevel != currentDifficultyLevel)
        {
            currentDifficultyLevel = newLevel;
            currentSpeedMultiplier = Mathf.Pow(GameConstants.DIFFICULTY_MULTIPLIER, currentDifficultyLevel - 1);
            currentSpeedMultiplier = Mathf.Min(currentSpeedMultiplier, GameConstants.MAX_FORWARD_SPEED / GameConstants.PLAYER_FORWARD_SPEED);

            // Update spawn rates based on difficulty
            if (obstacleSpawner != null)
                obstacleSpawner.SetDifficultyMultiplier(currentSpeedMultiplier);
        }
    }

    private void CheckGameOver()
    {
        if (playerController.IsAlive == false)
        {
            GameOver();
        }
    }

    public float GetSpeedMultiplier() => currentSpeedMultiplier;
    public int GetDifficultyLevel() => currentDifficultyLevel;
    public float GetGameTime() => gameTime;
}
