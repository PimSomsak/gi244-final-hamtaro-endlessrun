using System.Collections.Generic;
using UnityEngine;

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

