using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Spawns items (important + documents) and hazards at spawn points
/// that are children of generated room instances (e.g. LivingRoom, Kitchen).
/// </summary>
public class ObjectSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [Tooltip("All important items and documents prefabs")]
    public GameObject[] itemPrefabs;

    [Tooltip("All hazard prefabs")]
    public GameObject[] hazardPrefabs;

    [Header("Spawn Counts")]
    [Tooltip("How many items (important + documents) to spawn")]
    public int itemCount = 5;

    [Tooltip("How many hazards to spawn")]
    public int hazardCount = 2;

    [Header("Search Root")]
    [Tooltip("Root that contains all generated rooms (usually the GameObject with LayoutGenerator). If empty, uses this.transform.")]
    public Transform layoutGenerator;

    private readonly List<Transform> availableSpawnPoints = new List<Transform>();

    private void Start()
    {
        FindSpawnPointsFromRooms();
        SpawnAll();
    }

    /// <summary>
    /// Collect all spawn points under the configured root.
    /// Spawn points are children whose name starts with "SpawnPoint".
    /// </summary>
    private void FindSpawnPointsFromRooms()
    {
        availableSpawnPoints.Clear();

        Transform root = layoutGenerator != null ? layoutGenerator : transform;
        Transform[] allChildren = root.GetComponentsInChildren<Transform>(true);

        foreach (Transform t in allChildren)
        {
            if (t == root)
                continue;

            if (t.name.StartsWith("SpawnPoint"))
                availableSpawnPoints.Add(t);
        }

        if (availableSpawnPoints.Count == 0)
        {
            Debug.LogWarning(
                "[Spawner] No spawn points found. " +
                "Make sure your room prefabs have children named 'SpawnPoint_1', 'SpawnPoint_2', etc."
            );
        }
    }

    private void SpawnAll()
    {
        if (availableSpawnPoints.Count == 0)
            return;

        // Randomize spawn point order
        Shuffle(availableSpawnPoints);

        // Clamp requested counts to available spawn points
        int maxItemsPossible = Mathf.Min(itemCount, availableSpawnPoints.Count);
        int maxHazardsPossible = Mathf.Min(
            hazardCount,
            Mathf.Max(0, availableSpawnPoints.Count - maxItemsPossible)
        );

        int spawnIndex = 0;
        int spawnedItems = SpawnCategory(itemPrefabs, maxItemsPossible, ref spawnIndex, "Item");
        int spawnedHazards = SpawnCategory(hazardPrefabs, maxHazardsPossible, ref spawnIndex, "Hazard");

        int totalSpawned = spawnedItems + spawnedHazards;

        Debug.Log(
            $"[Spawner] Spawned {spawnedItems} items and {spawnedHazards} hazards " +
            $"using {totalSpawned} / {availableSpawnPoints.Count} spawn points."
        );
    }

    /// <summary>
    /// Spawn a category of prefabs into available spawn points.
    /// </summary>
    private int SpawnCategory(GameObject[] prefabs, int count, ref int spawnIndex, string categoryName)
    {
        if (prefabs == null || prefabs.Length == 0 || count <= 0)
            return 0;

        List<GameObject> pool = new List<GameObject>();
        AddValid(pool, prefabs);

        if (pool.Count == 0)
            return 0;

        int spawned = 0;

        for (int i = 0; i < count; i++)
        {
            if (spawnIndex >= availableSpawnPoints.Count)
            {
                Debug.LogWarning($"[Spawner] No more available spawn points for {categoryName}.");
                break;
            }

            GameObject chosenPrefab = pool[Random.Range(0, pool.Count)];
            Transform spawnPoint = availableSpawnPoints[spawnIndex];

            GameObject obj = Instantiate(
                chosenPrefab,
                spawnPoint.position,
                spawnPoint.rotation,
                transform
            );

            Debug.Log(
                $"[Spawner] Spawned {categoryName} '{obj.name}' at '{spawnPoint.name}' " +
                $"(position={spawnPoint.position})"
            );

            spawnIndex++;
            spawned++;
        }

        return spawned;
    }

    private void AddValid(List<GameObject> list, GameObject[] source)
    {
        if (source == null)
            return;

        foreach (GameObject g in source)
        {
            if (g != null && !list.Contains(g))
                list.Add(g);
        }
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int r = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[r];
            list[r] = temp;
        }
    }
}
