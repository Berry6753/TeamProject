using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueObjectPool : MonoBehaviour
{
    public static QueueObjectPool instance;

    [Header("풀링할 오브젝트 Prefab")]
    [SerializeField]
    private GameObject ObjectPrefab;

    private Queue<GameObject> pool = new Queue<GameObject>();
    private void Awake()
    {
        instance = this;
    }

    private GameObject Create()
    {
        GameObject newObject = Instantiate(ObjectPrefab);
        newObject.gameObject.SetActive(false);
        return newObject;
    }

    private void Inlit(int count)
    {
        for (int i = 0; i < count; i++)
        {
            pool.Enqueue(Create());
        }
    }

    public GameObject GetObject()
    {
        GameObject obj;
        if (instance.pool.Count > 0)
        {
            obj = instance.pool.Dequeue();            
        }
        else
        {
            obj = instance.Create();
        }

        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Relese(GameObject gameObject)
    {
        gameObject.SetActive(false);
        instance.pool.Enqueue(gameObject);
    }
}
