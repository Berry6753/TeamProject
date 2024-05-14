using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAttackState : TurretBaseState
{
    private float attackCheckTime;

    public TurretAttackState(Turret turret) : base(turret) { }

    public override void Enter()
    {
        turret.turretStateName = TurretStateName.ATTACK;
        turret.Attack();
    }

    public override void Update()
    {
        attackCheckTime += Time.time;

        turret.spinPos.transform.LookAt(turret.turretTargetTransform);

        if (attackCheckTime >= 60 / turret.turretAttackSpeed)
        {
            turret.Attack();
        }

        if(turret.turretTargetTransform == null /*||turret.turretTargetTransform.gameObject.GetComponent<Monster>().isDead*/)
        {
            //적찾기 상태로 변경
            turret.turretStatemachine.ChangeState(TurretStateName.SEARCH);
        }


    }

    public override void Exit()
    {
        attackCheckTime = 0;
    }


}
