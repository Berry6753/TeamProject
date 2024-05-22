using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBlueScreenState : TurretBaseState
{

    public TurretBlueScreenState(Turret turret) : base(turret) { }

    public override void Enter()
    {
        turret.OnRenderer();
        turret.turretStateName = TurretStateName.BLUESCREEN;
        //체크용 트리거 on
        //터렛의 콜라이더 off
        turret.turretCollider.enabled = false;
        //터렛의 태그와 레이어 없애기
        turret.transform.parent.gameObject.tag = "Untagged";
            turret.transform.parent.gameObject.layer = LayerMask.NameToLayer("debug");
        turret.gameObject.tag = "Untagged";
        turret.gameObject.layer = LayerMask.NameToLayer("debug");
        turret.turretCollider.gameObject.tag = "Untagged";
        turret.turretCollider.gameObject.layer = LayerMask.NameToLayer("debug");
        
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
        //터렛의 콜라이더 on
        turret.turretCollider.enabled = true;
        //터렛의 태그와 레이어 설정
        turret.transform.parent.gameObject.tag = "Turret";
        turret.transform.parent.gameObject.layer = LayerMask.NameToLayer("Turret");
        turret.gameObject.tag = "Turret";
        turret.gameObject.layer = LayerMask.NameToLayer("Turret");
        turret.turretCollider.gameObject.tag = "Turret";
        turret.turretCollider.gameObject.layer = LayerMask.NameToLayer("Turret");
    }
}
