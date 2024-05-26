using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnerCrystal_Last : MonsterSpawnCrystal
{
    private BossMonster bossMonster;

    protected override void Awake()
    {
        base.Awake();
        bossMonster = GameManager.Instance.Boss;
    }
    public override void Hurt(float damage)
    {
        if (!bossMonster.isDead) return;

        HP -= damage;
        if (HP <= 0)
        {
            monsterSpawner.DestorySpawner(this.transform);
            effect.SetActive(true);
            effect.transform.parent = null;
            gameObject.SetActive(false);
        }
    }
}
