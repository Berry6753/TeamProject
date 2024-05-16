using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSearchState : TurretBaseState
{

    private float searchTime = 0.5f;
    private float checkSearchTime;
    public TurretSearchState(Turret turret) : base(turret) { }

    public override void Enter()
    {
        turret.turretStateName = TurretStateName.SEARCH;
    }

    public override void Update()
    {
        checkSearchTime += Time.deltaTime;
        if(checkSearchTime > searchTime)
        {
            turret.SearchEnemy();
            checkSearchTime = 0;
        }

        if(turret.turretTargetTransform != null)
        {
            //공격 상태로 변환
            turret.turretStatemachine.ChangeState(TurretStateName.ATTACK);
        }
    }

    public override void Exit()
    {
        checkSearchTime = 0;
    }

}
