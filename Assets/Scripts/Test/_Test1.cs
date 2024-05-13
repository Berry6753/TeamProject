using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class _Test1 : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public bool isBuildAble;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

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
