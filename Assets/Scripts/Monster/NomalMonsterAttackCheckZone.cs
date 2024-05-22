using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalMonsterAttackCheckZone : MonoBehaviour
{
    private Monster monster;
    private void Awake()
    {
        monster = transform.GetComponentInParent<Monster>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer==LayerMask.NameToLayer("Turret")/*|| other.gameObject.layer == LayerMask.NameToLayer("Player")*/)
        {
            monster.isAttackAble = true;
        }
    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Turret") || other.gameObject.layer == LayerMask.NameToLayer("Player"))
    //    {
    //        monster.isAttackAble = true;
    //    }
    //}

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Turret") || other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("777777777777777777777777777777777777777");
            monster.isAttackAble = false;
        }
    }
}
