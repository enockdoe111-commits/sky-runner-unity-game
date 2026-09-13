// ObjectPool.cs - Object pooling system for performance optimization

using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int initialPoolSize = 20;

    private Queue<GameObject> availableObjects = new Queue<GameObject>();
    private HashSet<GameObject> activeObjects = new HashSet<GameObject>();

    public void Initialize(GameObject prefabToPool, int poolSize = 20)
    {
        prefab = prefabToPool;
        initialPoolSize = poolSize;
        
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            availableObjects.Enqueue(obj);
        }
    }

    public GameObject GetObject()
    {
        GameObject obj;

        if (availableObjects.Count > 0)
        {
            obj = availableObjects.Dequeue();
        }
        else
        {
            // Create new object if pool is empty
            obj = Instantiate(prefab, transform);
        }

        activeObjects.Add(obj);
        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        activeObjects.Remove(obj);
        availableObjects.Enqueue(obj);
    }

    public int GetAvailableCount() => availableObjects.Count;
    public int GetActiveCount() => activeObjects.Count;
}
