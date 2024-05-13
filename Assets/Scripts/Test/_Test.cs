using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Test : MonoBehaviour
{
    public bool isBuildAble;

    private void OnEnable()
    {
        transform.GetChild(0).GetComponent<Collider>().isTrigger = true;
        isBuildAble = true;
    }

    private void OnDisable()
    {
        gameObject.layer = LayerMask.NameToLayer("debug");
        gameObject.tag = "Untagged";
        transform.GetChild(0).tag = "Untagged";
        transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("debug");
        MultiObjectPool.ReturnToPool(gameObject);
    }
}
