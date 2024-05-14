using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDestroyState : TurretBaseState
{
    public TurretDestroyState(Turret turret) : base(turret) { }

    public override void Enter()
    {
        turret.turretStateName = TurretStateName.DESTROIY;
        //이펙트 재생
    }
}
