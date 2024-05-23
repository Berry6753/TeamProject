using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DamageBox : MonoBehaviour
{
    private Monster monsterScripts;

    private void Awake()
    {
        monsterScripts = GetComponentInParent<Monster>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Monster")) return;


        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player_Info>().Hurt(monsterScripts.monsterDamage);
        }else if (other.CompareTag("Turret"))
        {
            other.GetComponent<Turret>().Hurt(monsterScripts.monsterPower);
        }
        else if (other.CompareTag("Core"))
        {
            other.GetComponent<Core>().Hurt(monsterScripts.monsterPower);
            //Debug.Log(other.name + "АјАн Сп...");
        }
        else if (other.CompareTag("Barricade"))
        {
            other.GetComponent<Brricade>().Hurt(monsterScripts.monsterPower);
        }
    }
}
