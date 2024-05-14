using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SiegeMonster : Monster
{
    protected override void Awake()
    {
        base.Awake();
        defaltTarget = GameObject.FindWithTag("Core").GetComponent<Transform>();
        attack = GetComponentInChildren<SphereCollider>();
        attack.enabled = false;
    }
    private void Start()
    {
        chaseTarget = defaltTarget;
        ChaseTarget();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void ChaseTarget()
    {
        StartCoroutine(MonsterState());
    }

    protected override void SpawnTiming()
    {
        throw new NotImplementedException();
    }
}
