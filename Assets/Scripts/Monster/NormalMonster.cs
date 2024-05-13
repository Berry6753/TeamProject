using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonster : Monster
{
    private void Update()
    {
        Debug.Log(defaltTarget.position);
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
            PriorityTarget();
        }
    }
}