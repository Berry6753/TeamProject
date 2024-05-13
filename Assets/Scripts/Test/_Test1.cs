using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class _Test1 : MonoBehaviour
{
    public Rigidbody rig;
    public bool isBuildAble {  get; private set; }

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        rig.constraints = RigidbodyConstraints.None;
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

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Turret"))
        {
            isBuildAble = false;
            Debug.Log("ºôµå ºÒ°¡");
        }
        else
        {
            isBuildAble = true;
        }
    }
}
