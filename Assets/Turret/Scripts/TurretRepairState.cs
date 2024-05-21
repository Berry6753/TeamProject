using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRepairState : TurretBaseState
{
    private float checkTime;

    public TurretRepairState(Turret turret) : base(turret) { }

    public override void Enter()
    {
        turret.turretStateName = TurretStateName.REPAIR;
        //게이지 표시
        turret.sliderGage.gameObject.SetActive(true);
        turret.repairAudio.Play();
        turret.sliderGage.maxValue = turret.turretRepairTime;
    }

    public override void Update()
    {
        checkTime += Time.deltaTime;
        turret.sliderGage.value = checkTime;
        turret.sliderGage.transform.parent.forward = Camera.main.transform.forward;
        
        turret.repairAudio.pitch = Time.timeScale;
        if (checkTime >= turret.turretRepairTime)
        {
            //적찾기로 변경
            turret.turretStatemachine.ChangeState(TurretStateName.SEARCH);
        }
    }

    public override void Exit()
    {
        checkTime = 0;
        turret.repairAudio.Stop();
        turret.sliderGage.gameObject.SetActive(false);
        turret.Repair();
    }
}
