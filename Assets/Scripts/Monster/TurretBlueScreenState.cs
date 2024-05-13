using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBlueScreenState : TurretBaseState
{

    public TurretBlueScreenState(Turret turret) : base(turret) { }

    public override void Enter()
    {
        turret.turretStateName = TurretStateName.BLUESCREEN;
        //체크용 트리거 on
        //터렛의 콜라이더 off
        //터렛의 태그와 레이어 없애기
    }

    public override void Update()
    {
        turret.ChangeColor();
    }

    public override void Exit() 
    {
        turret.ResetColor();
        //체크용 트리거 off
        //터렛의 콜라이더 on
        //터렛의 태그와 레이어 설정
    }
}
