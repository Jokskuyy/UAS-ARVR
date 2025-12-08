using UnityEngine;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] importantItems;
    public GameObject[] documents;
    public GameObject[] hazards;

    public int spawnCount = 3;
    public BoxCollider spawnArea;

    void Start()
    {
        SpawnUniqueObjects();
    }

    void SpawnUniqueObjects()
    {
        List<GameObject> pool = new List<GameObject>();

        AddValid(pool, importantItems);
        AddValid(pool, documents);
        AddValid(pool, hazards);

        if (pool.Count < spawnCount)
        {
            Debug.LogError("Jumlah prefab kurang untuk spawn unik!");
            return;
        }

        // 🔀 Acak pool
        Shuffle(pool);

        // 🚀 Spawn N objek pertama (PASTI BEDA)
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 pos = GetRandomPoint();
            Instantiate(pool[i], pos, Quaternion.identity);
        }
    }

    void AddValid(List<GameObject> list, GameObject[] source)
    {
        if (source == null) return;

        foreach (GameObject g in source)
        {
            if (g != null && !list.Contains(g))
                list.Add(g);
        }
    }

    void Shuffle(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int r = Random.Range(i, list.Count);
            GameObject temp = list[i];
            list[i] = list[r];
            list[r] = temp;
        }
    }

    Vector3 GetRandomPoint()
    {
        Bounds b = spawnArea.bounds;

        float x = Random.Range(b.min.x, b.max.x);
        float z = Random.Range(b.min.z, b.max.z);

        return new Vector3(x, b.center.y, z);
    }
}
