using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurretDestroyState : TurretBaseState
{
    private float checkTime;
    public TurretDestroyState(Turret turret) : base(turret) { }

    public override void Enter()
    {
        checkTime = 0;
        turret.turretStateName = TurretStateName.DESTROIY;
    }

    public override void Update()
    {
        checkTime += Time.deltaTime;
        if(checkTime > 1)
        {
            turret.OffRenderer();
            turret.deathEffect.SetActive(true);
        }
        else if(checkTime > 1.5f) 
        {
            turret.transform.parent.gameObject.SetActive(false);
        }
    }
}
