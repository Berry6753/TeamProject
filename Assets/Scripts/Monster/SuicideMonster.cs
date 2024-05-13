using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SuicideMonster : Monster
{
    private void Update()
    {
        ChaseTarget();
    }

    protected override void ChaseTarget()
    {
        distance = Vector3.Distance(transform.position, defaltTarget.position);
        if (distance <= nav.stoppingDistance)
        {
            FreezeVelocity();
            //3초 뒤 폭발 구현
        }
        else
        {
            nav.SetDestination(defaltTarget.position);
        }
    }
}
