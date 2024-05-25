using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTurretAttackZone : MonoBehaviour
{
    [SerializeField]
    private GameObject parent;
    private Turret turret;
    private float checkTime;
    private void Awake()
    {
        turret =parent.GetComponent<Turret>();
    }

    private void Update()
    {
        if (turret.turretStateName != TurretStateName.ATTACK) 
        {
            gameObject.SetActive(false);
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if ( other.gameObject.layer==LayerMask.NameToLayer("Monster")) 
        {
            checkTime += Time.deltaTime;
            if (checkTime >= 1 / turret.turretAttackSpeed) 
            {
                if (other.gameObject.CompareTag("Monster"))
                {
                    other.gameObject.GetComponent<Monster>().Hurt(turret.turretAttackDamge);

                }

                if (other.gameObject.CompareTag("Boss"))
                {
                    other.gameObject.GetComponent<BossMonster>().Hurt(turret.turretAttackDamge);
                }

                checkTime = 0;
            }
            
        }
        else if (other.gameObject.CompareTag("Barrel"))
        {
            other.gameObject.GetComponent<Barrel>().Hurt();
        }


    }
}
