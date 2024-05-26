using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnCrystal : MonoBehaviour
{
    [Header("Ã¼·Â")]
    [SerializeField]
    private float HP;

    private MonsterSpawner monsterSpawner;

    private void Awake()
    {
        monsterSpawner = GameManager.Instance.MonsterSpawner;
    }

    public void Hurt(float damage)
    {
        HP -= damage;
        if(HP <= 0)
        {
            monsterSpawner.DestorySpawner(this.transform);
        }        
    }


}
