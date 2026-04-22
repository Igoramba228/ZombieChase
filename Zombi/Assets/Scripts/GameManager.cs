using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject zombiePrefab;

    [Header("UI")]
    public TMP_Text timerText;
    public TMP_Text healthText;
    public TMP_Text zombieCountText;
    public TMP_Text difficultyText;
    public GameObject gameOverPanel;
    public TMP_Text finalTimeText;
    public GameObject pausePanel;

    private float _timer;
    private float _spawnTimer;
    private bool _isGameOver;
    private bool _isPaused;
    private GameObject _currentPlayer;
    private int _initialZombies;
    private float _spawnInterval;
    private float _zombieSpeed;
    private float _playerSpeed;
    private string _difficulty;

    private float _minX = -11f;
    private float _maxX = 11f;
    private float _minY = -6f;
    private float _maxY = 6f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _initialZombies = PlayerPrefs.GetInt("InitialZombies", 2);
        _spawnInterval = PlayerPrefs.GetFloat("SpawnInterval", 5f);
        _zombieSpeed = PlayerPrefs.GetFloat("ZombieSpeed", 6f);
        _playerSpeed = PlayerPrefs.GetFloat("PlayerSpeed", 7f);
        _difficulty = PlayerPrefs.GetString("Difficulty", "Easy");

        _timer = 0f;
        _spawnTimer = 0f;
        _isGameOver = false;
        _isPaused = false;
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);

        if (difficultyText != null)
            difficultyText.text = $"Difficulty: {_difficulty}";

        Vector2 playerSpawn = new Vector2(_minX + 1.5f, _minY + 1.5f);
        _currentPlayer = Instantiate(playerPrefab, playerSpawn, Quaternion.identity);

        PlayerController pc = _currentPlayer.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.SetSpeed(_playerSpeed);

            if (_difficulty == "Hard")
            {
                pc.SetMaxHealth(5);
            }
        }

        for (int i = 0; i < _initialZombies; i++)
        {
            SpawnZombie();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (_isGameOver || _isPaused) return;

        _timer += Time.deltaTime;
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer >= _spawnInterval)
        {
            _spawnTimer = 0f;
            SpawnZombie();
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (timerText != null)
        {
            int minutes = (int)(_timer / 60);
            int seconds = (int)(_timer % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }

        if (healthText != null && _currentPlayer != null)
        {
            PlayerController pc = _currentPlayer.GetComponent<PlayerController>();
            healthText.text = $"HP: {pc.Health}";
        }

        if (zombieCountText != null)
        {
            int count = GameObject.FindGameObjectsWithTag("Zombie").Length;
            zombieCountText.text = $"Zombies: {count}";
        }
    }

    public void SpawnZombie()
    {
        Vector2 spawnPos;
        int attempts = 0;

        do
        {
            spawnPos = new Vector2(
                Random.Range(_maxX - 4f, _maxX - 1f),
                Random.Range(_maxY - 4f, _maxY - 1f)
            );
            attempts++;
        }
        while (IsPositionOccupied(spawnPos) && attempts < 50);

        GameObject zombie = Instantiate(zombiePrefab, spawnPos, Quaternion.identity);
        ZombieController zc = zombie.GetComponent<ZombieController>();
        if (zc != null)
        {
            zc.SetSpeed(_zombieSpeed);
        }
    }

    private bool IsPositionOccupied(Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 1f);
        foreach (var col in colliders)
        {
            if (col.CompareTag("Zombie") || col.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    public void GameOver()
    {
        _isGameOver = true;
        gameOverPanel.SetActive(true);

        int minutes = (int)(_timer / 60);
        int seconds = (int)(_timer % 60);
        finalTimeText.text = $"Time: {minutes:00}:{seconds:00}\nDifficulty: {_difficulty}";

        SaveBestScore();
    }

    private void SaveBestScore()
    {
        string key = _difficulty + "Best";
        float best = PlayerPrefs.GetFloat(key, 0f);

        if (_timer > best)
        {
            PlayerPrefs.SetFloat(key, _timer);
            PlayerPrefs.Save();
        }
    }

    public void TogglePause()
    {
        _isPaused = !_isPaused;
        pausePanel.SetActive(_isPaused);
        Time.timeScale = _isPaused ? 0f : 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        _isGameOver = false;
        _timer = 0f;
        _spawnTimer = 0f;
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);

        PlayerController[] players = Object.FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        foreach (var p in players) Destroy(p.gameObject);

        ZombieController[] zombies = Object.FindObjectsByType<ZombieController>(FindObjectsSortMode.None);
        foreach (var z in zombies) Destroy(z.gameObject);

        Vector2 playerSpawn = new Vector2(_minX + 1.5f, _minY + 1.5f);
        _currentPlayer = Instantiate(playerPrefab, playerSpawn, Quaternion.identity);

        PlayerController pc = _currentPlayer.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.SetSpeed(_playerSpeed);

            if (_difficulty == "Hard")
            {
                pc.SetMaxHealth(6);
            }
        }

        for (int i = 0; i < _initialZombies; i++)
        {
            SpawnZombie();
        }
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}