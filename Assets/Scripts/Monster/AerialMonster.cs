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
    //protected override void ChaseTarget()
    //{
    //    distance = Vector3.Distance(transform.position, defaltTarget.position);
    //    if (distance <= atttackRange)
    //    {
    //        rb.velocity = Vector3.zero;
    //    }
    //    else
    //    {
    //        PrioTarget();
    //    }
    //}

    //private void PrioTarget()
    //{
    //    if (turretTarget.Count > 0)
    //    {
    //        for (int i = 0; i < turretTarget.Count; i++)
    //        {
    //            transform.LookAt(turretTarget[i]);
    //            transform.position = Vector3.MoveTowards(transform.position, turretTarget[i].position, speed * Time.deltaTime);
    //            break;
    //        }
    //    }
    //    else
    //    {
    //        transform.LookAt(defaltTarget);
    //        transform.position = Vector3.MoveTowards(transform.position, defaltTarget.position, speed * Time.deltaTime);
    //    }
    //}
    protected override void ChaseTarget()
    {
        distance = Vector3.Distance(transform.position, defaltTarget.position);
    }
}
