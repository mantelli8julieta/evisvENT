using System.Collections.Generic;
using UnityEngine;

public class PoolSimple : MonoBehaviour
{
    public static PoolSimple Instance { get; private set; }

    [Header("Pool Settings")]
    public GameObject prefab;
    public int initial = 40;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        for (int i = 0; i < initial; i++)
        {
            GameObject go = Instantiate(prefab);
            go.SetActive(false);
            pool.Enqueue(go);
        }
    }

    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        GameObject go;
        if (pool.Count > 0) go = pool.Dequeue();
        else go = Instantiate(prefab);

        go.transform.position = position;
        go.transform.rotation = rotation;
        go.SetActive(true);
        return go;
    }

    public void Return(GameObject go)
    {
        go.SetActive(false);
        pool.Enqueue(go);
    }
}
