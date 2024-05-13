using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Test : MonoBehaviour
{

    private void OnDisable()
    {
        MultiObjectPool.ReturnToPool(gameObject);
    }
}
