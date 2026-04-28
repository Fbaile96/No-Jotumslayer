using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject basicEnemyPrefab;
    public GameObject fastEnemyPrefab;
    public GameObject tankEnemyPrefab;

    [Header("Configuración base")]
    public float spawnRadius = 25f;
    public float initialSpawnInterval = 2f;
    public int initialMaxEnemies = 30;

    [Header("Escalado de dificultad")]
    public float difficultyScaleInterval = 60f; // cada 60 segundos escala
    public float spawnIntervalReduction = 0.1f; // reduce el intervalo entre spawns
    public float minSpawnInterval = 0.3f;       // intervalo mínimo
    public int maxEnemiesIncrement = 10;         // enemigos extra por escala
    public int absoluteMaxEnemies = 150;         // tope máximo
    public float enemyHealthMultiplier = 0.15f;  // +15% vida por escala

    private float timer;
    private float elapsedTime;
    private float currentSpawnInterval;
    private int currentMaxEnemies;
    private float currentHealthMultiplier = 1f;
    private int difficultyLevel = 0;

    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
        currentMaxEnemies = initialMaxEnemies;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        timer += Time.deltaTime;

        // Escala la dificultad cada X segundos
        int newDifficultyLevel = Mathf.FloorToInt(elapsedTime / difficultyScaleInterval);
        if (newDifficultyLevel > difficultyLevel)
        {
            difficultyLevel = newDifficultyLevel;
            ScaleDifficulty();
        }

        if (timer >= currentSpawnInterval && CountEnemies() < currentMaxEnemies)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void ScaleDifficulty()
    {
        // Reduce el intervalo entre spawns
        currentSpawnInterval = Mathf.Max(
            currentSpawnInterval - spawnIntervalReduction,
            minSpawnInterval
        );

        // Aumenta el máximo de enemigos
        currentMaxEnemies = Mathf.Min(
            currentMaxEnemies + maxEnemiesIncrement,
            absoluteMaxEnemies
        );

        // Aumenta la vida de los enemigos
        currentHealthMultiplier += enemyHealthMultiplier;

        Debug.Log($"Dificultad {difficultyLevel}: Intervalo={currentSpawnInterval:F2}s, MaxEnemigos={currentMaxEnemies}, VidaMultiplicador={currentHealthMultiplier:F2}x");
    }

    void SpawnEnemy()
    {
        Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = new Vector3(randomCircle.x, 1f, randomCircle.y);

        float roll = Random.value;
        GameObject prefabToSpawn;

        if (roll < 0.5f) prefabToSpawn = basicEnemyPrefab;
        else if (roll < 0.8f) prefabToSpawn = fastEnemyPrefab;
        else prefabToSpawn = tankEnemyPrefab;

        GameObject enemyObj = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

        // Aplica el multiplicador de vida al enemigo spawneado
        EnemyBase enemy = enemyObj.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            enemy.maxHealth *= currentHealthMultiplier;
            enemy.currentHealth = enemy.maxHealth;
        }
    }

    int CountEnemies()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
}