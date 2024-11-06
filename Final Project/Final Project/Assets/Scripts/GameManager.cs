using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

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

    [SerializeField] private GameObject enemyShipPrefab;
    [SerializeField] private int enemySpawnChance = 30;
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private float obstacleSpawnTime = 2f;

    private int totalPoints;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnObstacle), 1f, obstacleSpawnTime);
    }

    private void SpawnObstacle()
    {
        int chance = Random.Range(0, 100);
        if (chance <= enemySpawnChance)
            SpawnEnemy();
        else
            SpawnAsteroid();
    }

    private void SpawnAsteroid()
    {
        GameObject asteroid = Instantiate(asteroidPrefab, RandomSpawn(), asteroidPrefab.transform.rotation);
        asteroid.GetComponent<Asteroid>().SetPoints(difficultyPoints[currentDifficulty]);
        asteroid.GetComponent<Asteroid>().OnDestroy.AddListener(AddPoints);
    }

    private void SpawnEnemy()
    {
        GameObject ship = Instantiate(enemyShipPrefab, RandomSpawn(), enemyShipPrefab.transform.rotation);
        ship.GetComponent<EnemyShip>().SetPoints(difficultyPoints[currentDifficulty]);
        ship.GetComponent<EnemyShip>().OnDeath.AddListener(AddPoints);
    }

    private Vector3 RandomSpawn()
    {
        float y = Random.Range(-9f, 9f);
        return new Vector3(25, y, 0);
    }

    public void AddPoints(int pts) => totalPoints += pts;
    public int GetPoints()
    {
        return totalPoints;
    }

    void Update()
    {
        gameTime += Time.deltaTime;

        for (int i = 0; i < difficultyThresholds.Length; i++)
        {
            if (gameTime > difficultyThresholds[i] && currentDifficulty < i + 1)
            {
                currentDifficulty = i + 1;
                IncreaseDifficulty();
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
