using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerialMonster : Monster
{
    [SerializeField] private float speed;
    [SerializeField] private float atttackRange;

    private void Update()
    {
        ChaseTarget();
    }
    protected override void ChaseTarget()
    {
        float distance = Vector3.Distance(transform.position, defaltTarget.position);
        if (distance <= atttackRange)
        {
            rb.velocity = Vector3.zero;
        }
        else
        {
            transform.LookAt(defaltTarget);
            transform.position = Vector3.MoveTowards(transform.position, defaltTarget.position, speed * Time.deltaTime);
            if (isChase)
                PrioTarget();
        }
    }

    private void PrioTarget()
    {
        for (int i = 0; i < priorityTarget.Length; i++)
        {
            if (priorityTarget[i] != null)
            {
                transform.LookAt(priorityTarget[i]);
                transform.position = Vector3.MoveTowards(transform.position, priorityTarget[i].position, speed * Time.deltaTime);
                break;
            }
        }
    }
}
