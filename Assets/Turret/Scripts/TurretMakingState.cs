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
        turret.sliderGage.gameObject.SetActive(true);
        turret.sliderGage.maxValue = turret.turretMakingTime;
        turret.sliderGage.transform.position = turret.transform.position;
        //만드는 이펙트 생성
        turret.makingEfect.SetActive(true);
        
    }

    public override void Update()
    {
        checkTime += Time.deltaTime;
        turret.sliderGage.value = checkTime;
        turret.sliderGage.transform.parent.forward = Camera.main.transform.forward;
        turret.makeAudio.pitch = Time.timeScale;
        //turret.sliderGage.transform.LookAt(Camera.main.transform.position); //플레이어 바라보게 아니면 카메라
        if (checkTime > turret.turretMakingTime)
        {
            //적찾기 상태로 변환
            turret.turretStatemachine.ChangeState(TurretStateName.SEARCH);
        }
    }

    public override void Exit()
    {
        checkTime = 0;
        turret.OnRenderer();
        turret.sliderGage.gameObject.SetActive(false);
        //만드는 이펙트 끄기
        turret.makingEfect.SetActive(false);
    }
}
