using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // increase the gameTime as the game progresses
    [SerializeField] private float gameTime = 0;

    // check the thresholds to see if we reach a new difficulty
    [SerializeField] private float[] difficultyThresholds;
    // if we do, increase the current difficulty
    [SerializeField] private int currentDifficulty = 0;

    // the worth of points in relation to the current difficulty
    [SerializeField] private int[] difficultyPoints;

    [SerializeField] private GameObject[] enemyShipPrefabs;
    [SerializeField] private int enemySpawnChance = 30;
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private float obstacleSpawnTime = 2f;

    [SerializeField] private GameObject playerShipPrefab;

    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private TextMeshProUGUI pointsUI;
    [SerializeField] private TextMeshProUGUI timeUI;
    [SerializeField] private TextMeshProUGUI highscoreUI;
    [SerializeField] private TextMeshProUGUI difficultyUI;
    [SerializeField] private RawImage[] healthUI;

    public UnityEvent GameOver;

    private int totalPoints;
    private int highestPoints = 0;

    private bool pauseGame = false;
    private bool gameRunning = false;
    private bool spawnObstacles = false;

    private const string HIGHSCORE = "highscore";

    private void Start()
    {
        titleScreen.SetActive(true);
        gameOverScreen.SetActive(false);
        pauseMenu.SetActive(false);

        pointsUI.enabled = false;
        timeUI.enabled = false;
        UpdateHealthUI(0);
        highscoreUI.enabled = false;
        difficultyUI.enabled = false;

        if (PlayerPrefs.HasKey(HIGHSCORE))
            highestPoints = PlayerPrefs.GetInt(HIGHSCORE);

        SoundManager.instance.TitleMusic(true);
    }

    public void StartGame()
    {
        gameTime = 0;
        currentDifficulty = 0;
        enemySpawnChance = 30;
        obstacleSpawnTime = 2;
        totalPoints = 0;

        gameRunning = true;
        spawnObstacles = true;
        pointsUI.enabled = true;
        timeUI.enabled = true;
        highscoreUI.enabled = true;
        difficultyUI.enabled = true;
        UpdateHealthUI(5);

        titleScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        SoundManager.instance.TitleMusic(false);
        SoundManager.instance.GameMusic(true);
        Instantiate(playerShipPrefab, Vector3.zero, Quaternion.identity);
        StartCoroutine(SpawnObstacles());
    }

    public void StopGame()
    {
        gameRunning = false;
        spawnObstacles = false;

        if (totalPoints > highestPoints)
            highestPoints = totalPoints;

        PlayerPrefs.SetInt(HIGHSCORE, highestPoints);

        GameOver.Invoke();

        gameOverScreen.SetActive(true);
        SoundManager.instance.TitleMusic(true);
        SoundManager.instance.GameMusic(false);
        StopCoroutine(SpawnObstacles());
    }

    public void Pause()
    {
        pauseGame = !pauseGame;
        if (pauseGame)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
    }

    public void QuitGame()
    {
        // for testing purposes
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        // for the exported game
        Application.Quit();
    }

    IEnumerator SpawnObstacles()
    {
        while (spawnObstacles)
        {
            int chance = Random.Range(0, 100);
            if (chance <= enemySpawnChance)
                SpawnEnemy();
            else
                SpawnAsteroid();

            yield return new WaitForSeconds(obstacleSpawnTime);
        }
    }

    private void SpawnAsteroid()
    {
        GameObject asteroid = Instantiate(asteroidPrefab, RandomSpawn(), asteroidPrefab.transform.rotation);
        Asteroid a = asteroid.GetComponent<Asteroid>();
        a.SetPoints(difficultyPoints[currentDifficulty]);
        a.OnDestroy.AddListener(AddPoints);
        GameOver.AddListener(a.Kill);
    }

    private void SpawnEnemy()
    {
        GameObject prefab = enemyShipPrefabs[Random.Range(0, enemyShipPrefabs.Length)];

        GameObject ship = Instantiate(prefab, RandomSpawn(), prefab.transform.rotation);
        EnemyShip e = ship.GetComponent<EnemyShip>();
        e.SetPoints(difficultyPoints[currentDifficulty]);
        e.OnDeath.AddListener(AddPoints);
        GameOver.AddListener(e.Kill);
    }

    private Vector3 RandomSpawn()
    {
        float y = Random.Range(-9f, 9f);
        return new Vector3(25, y, 0);
    }

    public void AddPoints(int pts)
    {
        totalPoints += pts;        
    }

    public int GetPoints()
    {
        return totalPoints;
    }

    public void UpdateHealthUI(int hp)
    {
        for (int i = 0; i < 5; i++)
        {
            if (i < hp)
                healthUI[i].enabled = true;
            else
                healthUI[i].enabled = false;
        }
    }

    void Update()
    {
        if (gameRunning)
        {
            pointsUI.text = "Points " + totalPoints;
            highscoreUI.text = "Highscore " + highestPoints;
            difficultyUI.text = "Difficulty " + (DifficultyLevel + 1);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Pause();
            }

            gameTime += Time.deltaTime;

            timeUI.text = "Time " + gameTime.ToString("0.0");

            for (int i = 0; i < difficultyThresholds.Length; i++)
            {
                if (gameTime > difficultyThresholds[i] && currentDifficulty < i + 1)
                {
                    currentDifficulty = i + 1;
                    IncreaseDifficulty();
                }
            }
        }
    }

    private void IncreaseDifficulty()
    {
        enemySpawnChance += 10;
        obstacleSpawnTime *= 0.75f;
    }

    public int DifficultyLevel => currentDifficulty;
    public float CurrentGameTime => gameTime;
}
