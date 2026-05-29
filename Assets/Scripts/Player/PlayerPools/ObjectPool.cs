using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private int initialSize = 50;
    [SerializeField] private bool canExpand = true;

    private readonly Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        for (int i = 0; i < initialSize; i++)
        {
            CreateObject();
        }
    }

    GameObject CreateObject()
    {
        GameObject obj = Instantiate(prefab, transform);
        obj.SetActive(false);
        pool.Enqueue(obj);
        return obj;
    }

    public GameObject Get()
    {
        if (pool.Count == 0)
        {
            if (!canExpand)
                return null;

            CreateObject();
        }

        GameObject obj = pool.Dequeue();
        obj.transform.SetParent(null);
        obj.SetActive(true);


        IPoolable poolable = obj.GetComponent<IPoolable>();
        poolable?.OnGetFromPool();

        return obj;
    }

    public void Return(GameObject obj)
    {
        if (obj == null)
            return;

        IPoolable poolable = obj.GetComponent<IPoolable>();
        poolable?.OnReturnToPool();

        obj.SetActive(false);
        obj.transform.SetParent(transform);

        pool.Enqueue(obj);
    }
}