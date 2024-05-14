using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelPos : MonoBehaviour
{
    private void OnDisable()
    {
        MultiObjectPool.ReturnToPool(gameObject);
    }
}
