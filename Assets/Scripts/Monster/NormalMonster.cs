using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonster : Monster
{
    protected override void Awake()
    {
        base.Awake();
        defaultTarget = GameObject.FindWithTag("Core").GetComponent<Transform>();
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

        Debug.Log($"{gameObject.name} ป๓ลย : {state}");
    }

    protected override void ChaseTarget()
    {
        StartCoroutine(MonsterState());
    }
}