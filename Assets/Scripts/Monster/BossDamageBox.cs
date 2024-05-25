using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamageBox : MonoBehaviour
{
    private BossMonster bossScripts;

    private void Awake()
    {
        bossScripts = GetComponentInParent<BossMonster>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Monster")) return;


        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player_Info>().Hurt(bossScripts.BossDamage);
        }
        else if (other.CompareTag("Turret"))
        {
            other.GetComponent<Turret>().Hurt(bossScripts.BossPower);
        }
        else if (other.CompareTag("Core"))
        {
            other.GetComponent<Core>().Hurt(bossScripts.BossPower);
            //Debug.Log(other.name + "АјАн Сп...");
        }
        else if (other.CompareTag("Barricade"))
        {
            other.GetComponent<Brricade>().Hurt(bossScripts.BossPower);
        }
    }
}
