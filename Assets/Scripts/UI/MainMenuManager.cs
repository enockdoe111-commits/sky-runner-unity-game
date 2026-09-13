// MainMenuManager.cs - Main menu scene manager

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Text highScoreText;
    [SerializeField] private Text totalCoinsText;

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            // Create AudioManager if it doesn't exist
            GameObject audioManagerObj = new GameObject("AudioManager");
            audioManager = audioManagerObj.AddComponent<AudioManager>();
        }

        Time.timeScale = 1f;

        // Setup button listeners
        if (startButton != null)
            startButton.onClick.AddListener(OnStartClicked);
        if (settingsButton != null)
            settingsButton.onClick.AddListener(OnSettingsClicked);
        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitClicked);

        // Display stats
        UpdateStats();

        audioManager.PlayBackgroundMusic();
    }

    private void UpdateStats()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);

        if (highScoreText != null)
            highScoreText.text = $"High Score: {highScore}";
        if (totalCoinsText != null)
            totalCoinsText.text = $"Total Coins: {totalCoins}";
    }

    private void OnStartClicked()
    {
        audioManager.PlaySFX("button");
        SceneManager.LoadScene("GamePlay");
    }

    private void OnSettingsClicked()
    {
        audioManager.PlaySFX("button");
        // TODO: Implement settings menu
        Debug.Log("Settings menu not implemented yet");
    }

    private void OnQuitClicked()
    {
        audioManager.PlaySFX("button");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
