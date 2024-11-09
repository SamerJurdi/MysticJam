using System.Collections;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float initialSpawnInterval = 5f;
    public float minSpawnInterval = 0.2f;
    public float spawnIntervalDecreaseRate = 0.2f;
    public float spawnDelayRange = 1f;
    public float difficultyTimer = 10f; // Time interval for the progressive spawn increase
    public float spawnMultiplier = 2f; // Multiplier for increasing enemies per spawn
    public float spawnPointsPercentage = 0.2f; // Percentage of spawn points to calculate the initial spawn count

    private Transform[] spawnPoints;
    private float currentSpawnInterval;
    private float timeSurvived; // How long the player has survived
    private int enemiesToSpawn; // Current number of enemies to spawn in one batch
    private float elapsedTime; // Time elapsed since last difficulty increase

    private void Start()
    {
        CollectSpawnPoints();
        currentSpawnInterval = initialSpawnInterval;
        enemiesToSpawn = Mathf.CeilToInt(spawnPointsPercentage * spawnPoints.Length); // Calculate the initial spawn rate
        elapsedTime = 0f; // Reset elapsed time
        StartCoroutine(SpawnEnemies());
    }

    private void Update()
    {
        // Track how long the player has survived
        timeSurvived += Time.deltaTime;

        // Gradually decrease the spawn interval as time goes on
        currentSpawnInterval = Mathf.Max(minSpawnInterval, initialSpawnInterval - spawnIntervalDecreaseRate * timeSurvived);

        // Track difficulty increase timing
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= difficultyTimer)
        {
            IncreaseDifficulty();
            elapsedTime = 0f; // Reset the timer for the next difficulty increase
        }
    }

    private void CollectSpawnPoints()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
        spawnPoints = System.Array.FindAll(spawnPoints, item => item != transform);
    }

    private void IncreaseDifficulty()
    {
        // Increase the number of enemies to spawn by a multiplier
        enemiesToSpawn = Mathf.CeilToInt(enemiesToSpawn * spawnMultiplier);
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Spawn the calculated number of enemies
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                // Randomly select a spawn point from the spawn points array
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

                // Random delay between spawns to prevent them from spawning at the same time
                float randomDelay = Random.Range(0f, spawnDelayRange);

                // Wait for the spawn interval plus a random delay
                yield return new WaitForSeconds(currentSpawnInterval + randomDelay);

                // Spawn the enemy at the selected spawn point
                Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            }

            // Optionally, we could adjust the total number of enemies to spawn after each batch if necessary
            yield return null;
        }
    }
}
