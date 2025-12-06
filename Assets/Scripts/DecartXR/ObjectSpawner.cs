using UnityEngine;
using System.Collections;

/// <summary>
/// Object Spawner - Launches objects toward the player to slice
/// </summary>
public class ObjectSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("Prefab of object to spawn (must have SliceableObject component)")]
    public GameObject objectPrefab;

    [Tooltip("How often to spawn objects (seconds)")]
    public float spawnInterval = 2f;

    [Tooltip("Random variation in spawn timing (±seconds)")]
    public float spawnVariation = 0.5f;

    [Header("Spawn Zone")]
    [Tooltip("Radius around spawner to spawn from")]
    public float spawnRadius = 3f;

    [Tooltip("Height range for spawn points")]
    public Vector2 spawnHeightRange = new Vector2(1f, 2.5f);

    [Header("Launch Settings")]
    [Tooltip("Target position to launch objects toward")]
    public Transform targetPosition;

    [Tooltip("Launch force multiplier")]
    public float launchForce = 10f;

    [Tooltip("Add random variance to launch direction")]
    public float launchVariance = 0.2f;

    [Header("Auto-Destroy")]
    [Tooltip("Destroy objects after this many seconds")]
    public float objectLifetime = 10f;

    private bool isSpawning = false;
    private int objectsSpawned = 0;

    void Start()
    {
        if (objectPrefab == null)
        {
            Debug.LogError("Object Prefab not assigned to spawner!");
            return;
        }

        if (targetPosition == null)
        {
            // Default to camera position
            targetPosition = Camera.main.transform;
        }

        StartSpawning();
    }

    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnRoutine());
            Debug.Log("Spawner started!");
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
        Debug.Log($"Spawner stopped. Total objects spawned: {objectsSpawned}");
    }

    private IEnumerator SpawnRoutine()
    {
        while (isSpawning)
        {
            SpawnObject();

            // Wait for next spawn with variation
            float waitTime = spawnInterval + Random.Range(-spawnVariation, spawnVariation);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void SpawnObject()
    {
        // Random spawn position around the spawner
        Vector3 spawnPos = GetRandomSpawnPosition();

        // Instantiate object
        GameObject obj = Instantiate(objectPrefab, spawnPos, Random.rotation);

        // Launch toward target
        LaunchObject(obj);

        // Auto-destroy after lifetime
        Destroy(obj, objectLifetime);

        objectsSpawned++;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        // Random angle around spawner
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float distance = Random.Range(spawnRadius * 0.5f, spawnRadius);

        // Random height
        float height = Random.Range(spawnHeightRange.x, spawnHeightRange.y);

        Vector3 offset = new Vector3(
            Mathf.Cos(angle) * distance,
            height,
            Mathf.Sin(angle) * distance
        );

        return transform.position + offset;
    }

    private void LaunchObject(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogWarning("Spawned object has no Rigidbody!");
            return;
        }

        // Calculate direction to target
        Vector3 directionToTarget = (targetPosition.position - obj.transform.position).normalized;

        // Add random variance
        Vector3 variance = new Vector3(
            Random.Range(-launchVariance, launchVariance),
            Random.Range(-launchVariance, launchVariance),
            Random.Range(-launchVariance, launchVariance)
        );

        Vector3 launchDirection = (directionToTarget + variance).normalized;

        // Apply force
        rb.AddForce(launchDirection * launchForce, ForceMode.Impulse);

        // Add some spin for visual effect
        rb.AddTorque(Random.insideUnitSphere * 2f, ForceMode.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw spawn radius
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);

        // Draw height range
        Gizmos.color = Color.yellow;
        Vector3 minHeight = transform.position + Vector3.up * spawnHeightRange.x;
        Vector3 maxHeight = transform.position + Vector3.up * spawnHeightRange.y;
        Gizmos.DrawLine(minHeight, maxHeight);

        // Draw target direction
        if (targetPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, targetPosition.position);
        }
    }

    private void OnValidate()
    {
        if (spawnInterval < 0.1f) spawnInterval = 0.1f;
        if (spawnRadius < 0.5f) spawnRadius = 0.5f;
        if (launchForce < 1f) launchForce = 1f;
    }
}
