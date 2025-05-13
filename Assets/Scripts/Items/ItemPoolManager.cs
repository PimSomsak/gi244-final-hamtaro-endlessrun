using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemPoolManager : MonoBehaviour
{
    [System.Serializable]
    public class SpawnableItem
    {
        public string name;
        public GameObject prefab;
        [Range(0, 100)]
        public float spawnChance;
        public int initialPoolSize = 5;

        [HideInInspector]
        public Queue<GameObject> pool = new Queue<GameObject>();
    }

    [Header("Item Settings")]
    public List<SpawnableItem> itemsToSpawn;

    [Header("Spawn Settings")]
    public Transform[] spawnPoints;
    public float spawnCooldown = 2f;

    void Start()
    {
        InitializePools();
        StartCoroutine(SpawnLoop());
    }

    void InitializePools()
    {
        foreach (var item in itemsToSpawn)
        {
            for (int i = 0; i < item.initialPoolSize; i++)
            {
                GameObject obj = Instantiate(item.prefab);
                obj.SetActive(false);
                item.pool.Enqueue(obj);
            }
        }
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnRandomItem();
            yield return new WaitForSeconds(spawnCooldown);
        }
    }

    public void SpawnRandomItem()
    {
        float roll = Random.Range(0f, 100f);
        float cumulative = 0f;

        foreach (var item in itemsToSpawn)
        {
            cumulative += item.spawnChance;
            if (roll <= cumulative)
            {
                SpawnFromPool(item);
                return;
            }
        }
    }

    void SpawnFromPool(SpawnableItem item)
    {
        if (item.pool.Count > 0)
        {
            GameObject obj = item.pool.Dequeue();
            int randomSpawn = Random.Range(0, spawnPoints.Length);
            obj.transform.position = spawnPoints[randomSpawn].position;
            obj.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"⚠️ Pool for {item.name} is empty!");
        }
    }

    public void ReturnToPool(GameObject obj, string itemName)
    {
        var item = itemsToSpawn.Find(x => x.name == itemName);
        if (item != null)
        {
            obj.SetActive(false);
            item.pool.Enqueue(obj);
        }
    }
}
