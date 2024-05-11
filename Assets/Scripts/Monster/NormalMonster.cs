using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonster : Monster
{
    float distance;
    private void Update()
    {
        ChaseTarget();
    }

    protected override void ChaseTarget()
    {
        distance = Vector3.Distance(transform.position, defaltTarget.position);
        if( distance <= nav.stoppingDistance )
        {
            FreezeVelocity();
        }
        else
        {
            nav.SetDestination(defaltTarget.position);
            if (isChase)
                PriorityTarget();
        }
    }
}