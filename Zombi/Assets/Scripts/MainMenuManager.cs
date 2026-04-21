using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainPanel;
    public GameObject difficultyPanel;
    public GameObject scoreboardPanel;

    [Header("Difficulty Settings")]
    public TMP_Text easyScoreText;
    public TMP_Text mediumScoreText;
    public TMP_Text hardScoreText;

    public static int initialZombies = 2;
    public static float spawnInterval = 5f;
    public static float zombieSpeed = 6f;
    public static float playerSpeed = 7f;
    public static string currentDifficulty = "Easy";

    private void Start()
    {
        ShowMainPanel();
        UpdateScoreboardUI();
    }

    public void ShowMainPanel()
    {
        mainPanel.SetActive(true);
        difficultyPanel.SetActive(false);
        scoreboardPanel.SetActive(false);
    }

    public void ShowDifficultyPanel()
    {
        mainPanel.SetActive(false);
        difficultyPanel.SetActive(true);
        scoreboardPanel.SetActive(false);
    }

    public void ShowScoreboardPanel()
    {
        mainPanel.SetActive(false);
        difficultyPanel.SetActive(false);
        scoreboardPanel.SetActive(true);
        UpdateScoreboardUI();
    }

    public void SelectEasy()
    {
        initialZombies = 2;
        spawnInterval = 5f;
        zombieSpeed = 5f;
        playerSpeed = 7f;
        currentDifficulty = "Easy";
        PlayerPrefs.SetString("Difficulty", "Easy");
    }

    public void SelectMedium()
    {
        initialZombies = 4;
        spawnInterval = 3f;
        zombieSpeed = 6f;
        playerSpeed = 8f;
        currentDifficulty = "Medium";
        PlayerPrefs.SetString("Difficulty", "Medium");
    }

    public void SelectHard()
    {
        initialZombies = 5;
        spawnInterval = 1.5f;
        zombieSpeed = 7f;
        playerSpeed = 9f;
        currentDifficulty = "Hard";
        PlayerPrefs.SetString("Difficulty", "Hard");
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("InitialZombies", initialZombies);
        PlayerPrefs.SetFloat("SpawnInterval", spawnInterval);
        PlayerPrefs.SetFloat("ZombieSpeed", zombieSpeed);
        PlayerPrefs.SetFloat("PlayerSpeed", playerSpeed);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }

    public void UpdateScoreboardUI()
    {
        if (easyScoreText != null)
            easyScoreText.text = $"Easy: {PlayerPrefs.GetFloat("EasyBest", 0f):F1}s";

        if (mediumScoreText != null)
            mediumScoreText.text = $"Medium: {PlayerPrefs.GetFloat("MediumBest", 0f):F1}s";

        if (hardScoreText != null)
            hardScoreText.text = $"Hard: {PlayerPrefs.GetFloat("HardBest", 0f):F1}s";
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}