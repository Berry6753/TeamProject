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
        turret.turretTargetTransform = null;
    }

    public override void Update()
    {
        checkSearchTime += Time.deltaTime;
        if(checkSearchTime > searchTime)
        {
            checkSearchTime = 0;
            turret.SearchEnemy();
        }

        
    }

    public override void Exit()
    {
        checkSearchTime = 0;
    }

}
