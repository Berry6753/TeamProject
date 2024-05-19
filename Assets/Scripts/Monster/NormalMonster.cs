using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonster : Monster
{
    protected override void Awake()
    {
        base.Awake();
        defaultTarget = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }
    private void Start()
    {
        ChaseTarget();
    }

    protected override void Update()
    {
        base.Update();
        PriorityTarget();
        LookAt();
    }

    protected override void ChaseTarget()
    {
        StartCoroutine(MonsterState());
    }
}