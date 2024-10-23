using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // increase the gameTime as the game progresses
    [SerializeField] private float gameTime = 0;

    // check the thresholds to see if we reach a new difficulty
    [SerializeField] private float[] difficultyThresholds;
    // if we do, increase the current difficulty
    [SerializeField] private int currentDifficulty = 0;

    [SerializeField] private GameObject enemyShipPrefab;
    [SerializeField] private int enemySpawnChance = 30;
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private float obstacleSpawnTime = 2f;

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
        Instantiate(asteroidPrefab, RandomSpawn(), asteroidPrefab.transform.rotation);
    }

    private void SpawnEnemy()
    {
        Instantiate(enemyShipPrefab, RandomSpawn(), enemyShipPrefab.transform.rotation);
    }

    private Vector3 RandomSpawn()
    {
        float y = Random.Range(-9f, 9f);
        return new Vector3(25, y, 0);
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
