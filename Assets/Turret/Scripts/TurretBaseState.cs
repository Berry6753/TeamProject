using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBaseState : BaseState
{
    protected Turret turret;

    public TurretBaseState(Turret turret)
    {
        this.turret = turret;
    }
}
