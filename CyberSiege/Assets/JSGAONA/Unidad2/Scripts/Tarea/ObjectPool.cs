using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    private Dictionary<GameObject, Queue<GameObject>> poolDict = new();

    private void Awake()
    {
        Instance = this;
    }

    public GameObject GetFromPool(GameObject prefab)
    {
        if (!poolDict.ContainsKey(prefab))
        {
            poolDict[prefab] = new Queue<GameObject>();
        }

        if (poolDict[prefab].Count > 0)
        {
            GameObject obj = poolDict[prefab].Dequeue();
            return obj;
        }

        return Instantiate(prefab);
    }

    public void ReturnToPool(GameObject prefab, GameObject instance)
    {
        instance.SetActive(false);
        poolDict[prefab].Enqueue(instance);
    }
}
