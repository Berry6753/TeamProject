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
        attackCheckTime += Time.deltaTime;

        turret.spinPos.transform.LookAt(turret.turretTargetTransform);


        if (attackCheckTime >= 1/turret.turretAttackSpeed)
        {
            
            turret.Attack();
            attackCheckTime = 0;
        }

        if (turret.turretTargetTransform.gameObject.activeSelf == false /*||turret.turretTargetTransform.gameObject.GetComponent<Monster>().isDead*/)
        {
            //적찾기 상태로 변경
            turret.turretStatemachine.ChangeState(TurretStateName.SEARCH);
        }


    }

    public override void Exit()
    {
        attackCheckTime = 0;
        turret.turretTargetTransform = null;
        turret.fireEfect.SetActive(false);
    }


}
