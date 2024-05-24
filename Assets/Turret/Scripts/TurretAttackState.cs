using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAttackState : TurretBaseState
{
    private float attackCheckTime;

    public TurretAttackState(Turret turret) : base(turret) { }

    public override void Enter()
    {
        turret.turretStateName = TurretStateName.ATTACK;
        if (turret.turretTargetTransform != null && turret.turretTargetTransform.gameObject.activeSelf)
            return;

        if (attackCheckTime >= 1 / turret.turretAttackSpeed)
        {

            turret.Attack();

            attackCheckTime = 0;
        }

    }

    public override void Update()
    {
        if (turret.targetIndex >= turret.turretTargetList.Count)
        {
            turret.turretStatemachine.ChangeState(TurretStateName.SEARCH);
            return;
        }
        attackCheckTime += Time.deltaTime;

        turret.spinPos.transform.LookAt(turret.turretTargetTransform);

        //���� ���� �� �Ҹ��� ����
        //Audio Manager���� ó�� 
        //���� ����׿�
        turret.fireAudio.pitch = Time.timeScale;

        if (turret.turretTargetList[turret.targetIndex] == null || !turret.turretTargetList[turret.targetIndex].gameObject.activeSelf)
        {
            turret.targetIndex++;
            // targetTransform = targetList[targetIndex].transform;
        }

        if (Vector3.Distance(turret.transform.position, turret.turretTargetTransform.transform.position) > turret.turretAttackRange/*||turret.turretTargetTransform.gameObject.GetComponent<Monster>().isDead*/)
        {
            turret.targetIndex++;
        }

        

        if (attackCheckTime >= 1/turret.turretAttackSpeed)
        {
            
            turret.Attack();
            
            attackCheckTime = 0;
        }

        

       
        


    }

    public override void Exit()
    {
        if (turret.fireAudio.loop == true)
        {
            turret.fireAudio.Stop();
        }
        turret.fireEfect.SetActive(false);
    }


}
