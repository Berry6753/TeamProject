using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMakingState : TurretBaseState
{
    private float checkTime;
    public TurretMakingState(Turret turret) : base(turret) { }

    public override void Enter()
    {
        turret.turretStateName = TurretStateName.MAKING;
        //만드는 이펙트 생성
        turret.makingEfect.SetActive(true);
    }

    public override void Update()
    {
        checkTime += Time.deltaTime;

        if(checkTime > turret.turretMakingTime)
        {
            //적찾기 상태로 변환
            turret.turretStatemachine.ChangeState(TurretStateName.SEARCH);
        }
    }

    public override void Exit()
    {
        checkTime = 0;
        turret.OnRenderer();
        //만드는 이펙트 끄기
        turret.makingEfect.SetActive(false);
    }
}
