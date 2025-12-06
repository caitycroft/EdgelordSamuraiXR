using UnityEngine;

/// <summary>
/// Random Fruit Spawner - Spawns different types of fruit to slice
/// </summary>
public class RandomFruitSpawner : MonoBehaviour
{
    [Header("Fruit Prefabs")]
    [Tooltip("Array of different fruit prefabs to spawn")]
    public GameObject[] fruitPrefabs;

    [Header("Spawn Timing")]
    [Tooltip("How often to spawn fruit (seconds)")]
    public float spawnInterval = 1.5f;

    [Tooltip("Random variation in spawn timing (±seconds)")]
    public float spawnVariation = 0.3f;

    [Header("Spawn Zone")]
    [Tooltip("Radius around spawner")]
    public float spawnRadius = 2f;

    [Tooltip("Height range for spawn points (min, max)")]
    public Vector2 spawnHeightRange = new Vector2(1f, 2.5f);

    [Header("Launch Settings")]
    [Tooltip("Where to aim the fruit")]
    public Transform targetPosition;

    [Tooltip("How hard to throw the fruit")]
    public float launchForce = 6f;

    [Tooltip("Random variance in launch direction")]
    public float launchVariance = 0.3f;

    [Header("Gameplay")]
    [Tooltip("Auto-destroy fruit after this many seconds")]
    public float fruitLifetime = 10f;

    [Tooltip("Start spawning automatically")]
    public bool autoStart = true;

    private float nextSpawnTime;
    private bool isSpawning = false;
    private int totalFruitSpawned = 0;

    void Start()
    {
        if (fruitPrefabs == null || fruitPrefabs.Length == 0)
        {
            Debug.LogError("No fruit prefabs assigned to RandomFruitSpawner!");
            return;
        }

        if (targetPosition == null)
        {
            // Default to main camera
            targetPosition = Camera.main.transform;
            Debug.Log("No target assigned, using main camera");
        }

        if (autoStart)
        {
            StartSpawning();
        }
    }

    void Update()
    {
        if (isSpawning && Time.time >= nextSpawnTime)
        {
            SpawnRandomFruit();
            ScheduleNextSpawn();
        }
    }

    public void StartSpawning()
    {
        isSpawning = true;
        nextSpawnTime = Time.time + spawnInterval;
        Debug.Log("Fruit spawner started!");
    }

    public void StopSpawning()
    {
        isSpawning = false;
        Debug.Log($"Fruit spawner stopped. Total fruit spawned: {totalFruitSpawned}");
    }

    private void SpawnRandomFruit()
    {
        // Pick random fruit from array
        GameObject fruitPrefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];

        // Calculate random spawn position
        Vector3 spawnPos = GetRandomSpawnPosition();

        // Instantiate the fruit
        GameObject fruit = Instantiate(fruitPrefab, spawnPos, Random.rotation);

        // Launch it toward the target
        LaunchFruit(fruit, spawnPos);

        // Auto-destroy after lifetime
        Destroy(fruit, fruitLifetime);

        totalFruitSpawned++;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        // Random angle around the spawner (360 degrees)
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

        // Random distance from center
        float distance = Random.Range(spawnRadius * 0.5f, spawnRadius);

        // Random height within range
        float height = Random.Range(spawnHeightRange.x, spawnHeightRange.y);

        // Calculate world position
        Vector3 offset = new Vector3(
            Mathf.Cos(angle) * distance,
            height,
            Mathf.Sin(angle) * distance
        );

        return transform.position + offset;
    }

    private void LaunchFruit(GameObject fruit, Vector3 spawnPos)
    {
        Rigidbody rb = fruit.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogWarning($"Fruit prefab {fruit.name} has no Rigidbody!");
            return;
        }

        // Calculate direction to target
        Vector3 directionToTarget = (targetPosition.position - spawnPos).normalized;

        // Add random variance to make it less predictable
        Vector3 variance = new Vector3(
            Random.Range(-launchVariance, launchVariance),
            Random.Range(-launchVariance * 0.5f, launchVariance), // Less variance in Y
            Random.Range(-launchVariance, launchVariance)
        );

        Vector3 finalDirection = (directionToTarget + variance).normalized;

        // Launch!
        rb.AddForce(finalDirection * launchForce, ForceMode.Impulse);

        // Add spin for realism
        rb.AddTorque(Random.insideUnitSphere * 2f, ForceMode.Impulse);
    }

    private void ScheduleNextSpawn()
    {
        float variation = Random.Range(-spawnVariation, spawnVariation);
        nextSpawnTime = Time.time + spawnInterval + variation;
    }

    // Visualize spawn area in editor
    private void OnDrawGizmosSelected()
    {
        // Draw spawn radius
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
        Gizmos.DrawWireSphere(transform.position, spawnRadius * 0.5f);

        // Draw height range
        Gizmos.color = Color.yellow;
        Vector3 minHeight = transform.position + Vector3.up * spawnHeightRange.x;
        Vector3 maxHeight = transform.position + Vector3.up * spawnHeightRange.y;
        Gizmos.DrawLine(minHeight - Vector3.right, minHeight + Vector3.right);
        Gizmos.DrawLine(maxHeight - Vector3.right, maxHeight + Vector3.right);
        Gizmos.DrawLine(minHeight, maxHeight);

        // Draw line to target
        if (targetPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, targetPosition.position);
        }
    }

    private void OnValidate()
    {
        // Ensure values are reasonable
        if (spawnInterval < 0.1f) spawnInterval = 0.1f;
        if (spawnRadius < 0.5f) spawnRadius = 0.5f;
        if (launchForce < 1f) launchForce = 1f;
        if (fruitLifetime < 1f) fruitLifetime = 1f;
    }

    // Public API for controlling spawner
    public int GetTotalSpawned() => totalFruitSpawned;
    public bool IsSpawning() => isSpawning;
    public void SetSpawnInterval(float interval) => spawnInterval = Mathf.Max(0.1f, interval);
    public void SetLaunchForce(float force) => launchForce = Mathf.Max(1f, force);
}
