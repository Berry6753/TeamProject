using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonster : Monster
{
    protected override void Awake()
    {
        base.Awake();
        defaltTarget = GameObject.FindWithTag("Player").GetComponent<Transform>();
        attack = GetComponentInChildren<SphereCollider>();
        attack.enabled = false;
    }
    private void Start()
    {
        ChaseTarget();
    }

    protected override void Update()
    {
        base.Update();
        PriorityTarget();
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