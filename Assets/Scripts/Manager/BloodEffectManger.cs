using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEffectManger : Singleton<BloodEffectManger>
{
    [SerializeField]
    private GameObject effectPrefab;

    private Queue<GameObject> bloodePool = new Queue<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < 300; ++i)
        {
            GameObject effect=Instantiate(effectPrefab);
            bloodePool.Enqueue(effect);
            effect.SetActive(false);
        }
    }


    public void UsingEffect(Vector3 transform)
    {
        GameObject effect= bloodePool.Dequeue();
        effect.SetActive(true);
        effect.transform.position = transform;
        effect.transform.forward = Camera.main.transform.forward;
        bloodePool.Enqueue(effect);
    }

}
