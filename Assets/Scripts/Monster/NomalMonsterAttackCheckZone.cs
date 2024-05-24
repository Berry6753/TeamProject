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

    private void OnEnable()
    {
        this.gameObject.layer = LayerMask.NameToLayer("CheckZone");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer==LayerMask.NameToLayer("Turret")&& other.gameObject.CompareTag("Barrel")/*|| other.gameObject.layer == LayerMask.NameToLayer("Player")*/)
        {
            monster.isAttackAble = true;
            monster.nav.isStopped = true;
            monster.nav.enabled = false;
            monster.obstacle.enabled = true;
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Player")&&monster.checkChasehaseTarget.gameObject.layer== LayerMask.NameToLayer("Player")&&!monster.isAttackAble)
        {
            monster.isAttackAble = true;
            monster.nav.isStopped = true;
            monster.nav.enabled = false;
            monster.obstacle.enabled = true;
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
        if (other.gameObject.layer == LayerMask.NameToLayer("Turret"))
        {
            monster.isAttackAble = false;
            monster.obstacle.enabled = false;
            monster.nav.enabled = true;
            monster.nav.isStopped = false;
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Player") && monster.checkChasehaseTarget.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            monster.isAttackAble = false;
            monster.obstacle.enabled = false;
            monster.nav.enabled = true;
            monster.nav.isStopped = false;
        }
    }
}
