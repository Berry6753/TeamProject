using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1 : MonoBehaviour
{
    private void OnDisable()
    {
        MultiObjectPool.ReturnToPool(gameObject);
    }
}
