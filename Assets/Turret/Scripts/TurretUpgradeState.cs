using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretUpgradeState : TurretBaseState
{
    private float checkTime;
    public TurretUpgradeState(Turret turret) : base(turret) { }

    public override void Enter()
    {
        turret.turretStateName = TurretStateName.UPGRADE;
        turret.OffRenderer();
        //만드는 이펙트 생성
        turret.makingEfect.SetActive(true);
    }

    public override void Update()
    {
        checkTime += Time.deltaTime;

        if (checkTime >= turret.turretUpgradeTime)
        {
            //적찾기 상태로  변경
            turret.turretStatemachine.ChangeState(TurretStateName.SEARCH);
        }
    }

    public override void Exit()
    {
        checkTime = 0;
        turret.Upgrade();
        turret.OnRenderer();
        turret.makingEfect.SetActive(false);
    }
}
