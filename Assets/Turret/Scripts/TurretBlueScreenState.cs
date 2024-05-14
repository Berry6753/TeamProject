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
        turret.checkCollider.enabled = true;
        //터렛의 콜라이더 off
        turret.turretCollider.enabled = false;
        //터렛의 태그와 레이어 없애기
        turret.gameObject.tag = "Untagged";
        turret.gameObject.layer = 0;
    }

    public override void Update()
    {
        turret.ChangeColor();
    }

    public override void Exit() 
    {
        turret.ResetColor();
        turret.OffRenderer();
        //체크용 트리거 off
        turret.checkCollider.enabled = false;
        //터렛의 콜라이더 on
        turret.turretCollider.enabled = true;
        //터렛의 태그와 레이어 설정
        turret.gameObject.tag = "Turret";
        turret.gameObject.layer = 8;
    }
}
