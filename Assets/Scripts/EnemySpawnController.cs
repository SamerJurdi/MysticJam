using System.Collections;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float initialSpawnInterval = 5f;
    public float minSpawnInterval = 0.2f;
    public float spawnIntervalDecreaseRate = 0.2f;
    public float spawnDelayRange = 1f;

    private Transform[] spawnPoints;
    private float currentSpawnInterval;
    private float timeSurvived; // How long the player has survived

    private void Start()
    {
        CollectSpawnPoints();
        currentSpawnInterval = initialSpawnInterval;
        StartCoroutine(SpawnEnemies());
    }

    private void Update()
    {
        // Track how long the player has survived
        timeSurvived += Time.deltaTime;

        // Gradually decrease the spawn interval as time goes on
        currentSpawnInterval = Mathf.Max(minSpawnInterval, initialSpawnInterval - spawnIntervalDecreaseRate * timeSurvived);
    }

    private void CollectSpawnPoints()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
        spawnPoints = System.Array.FindAll(spawnPoints, item => item != transform);
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Randomly select a spawn point from the spawn points array
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Random delay between spawn points to prevent them from spawning at the same time
            float randomDelay = Random.Range(0f, spawnDelayRange);

            // Wait for the spawn interval plus a random delay
            yield return new WaitForSeconds(currentSpawnInterval + randomDelay);

            // Spawn the enemy at the selected spawn point
            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}
