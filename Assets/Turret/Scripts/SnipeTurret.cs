using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnipeTurret : Turret
{
    [SerializeField]
    private GameObject bullet;

    private int nowSnipeTurretUpgradeCount = 0;
    private int nowSnipeTurretHp;
    private int maxSnipeTurretHp = 5;
    private int snipeTurretHpRise = 1;
    private float snipeTurretMakingTime = 15;
    private float snipeTurretAttackDamge = 100;
    private float snipeTurretAttackSpeed = 0.35f;
    private float snipeTurretAttackRange = 32;
    private float snipeTurretUpgradeTime = 15;
    private float snipeTurretRepairTime = 15;
    private float snipeTurretAttackRise = 50;
    private float snipeTurretAttackSpeedRise = 0.2f;
    private float snipeTurretUpgradCostRise = 1.5f;
    private float snipeTurretMaxUpgradeCount = 3;
    private float snipeTurretRepairCost = 10;
    private float snipeTurretUpgradeCost = 20;
    private float snipeTurretMakingCost = 30;

    
    

    protected override void OnEnable()
    {
        base.OnEnable();
        base.SetTurret(snipeTurretMakingTime, snipeTurretMakingCost, snipeTurretAttackDamge, snipeTurretAttackSpeed, snipeTurretAttackRange, maxSnipeTurretHp, snipeTurretHpRise, snipeTurretUpgradeCost, snipeTurretUpgradeTime, snipeTurretRepairTime, snipeTurretRepairCost, snipeTurretAttackRise,
            snipeTurretAttackSpeedRise, snipeTurretUpgradCostRise, snipeTurretMaxUpgradeCount);
        bullet.SetActive(false);
    }

    public override void Attack()
    {
        if (targetIndex >= turretTargetList.Count)
        {
            return;
        }

        targetTransform = targetList[targetIndex].transform;

        if (Physics.Raycast(firePos.transform.position, fireEffectPos.transform.position - firePos.transform.position,out RaycastHit hit, snipeTurretAttackRange,~(ignoreLayer)))
        {
            if (!hit.collider.CompareTag("Monster"))
            {
                targetIndex++;
                return;
                //if (targetIndex >= targetList.Count )
                //{
                //    turretStatemachine.ChangeState(TurretStateName.SEARCH);
                //}
                //if (targetIndex < targetList.Count) 
                //{
                //    targetTransform = targetList[targetIndex].transform;
                //}

            }

            fireEfect.SetActive(true);
            firePaticle.Play();
            fireAudio.Play();

            bullet.SetActive(true);

            //RaycastHit[] raycastHits = Physics.RaycastAll(firePos.transform.position, fireEffectPos.transform.position - firePos.transform.position, snipeTurretAttackRange);

            //foreach (RaycastHit monster in raycastHits)
            //{


            //    if (hit.collider.CompareTag("Monster"))
            //    {
            //        //����Ʈ ����
            //        //���� ������ �ִ� �κ�
            //        hit.collider.gameObject.GetComponent<Monster>().Hurt(base.turretAttackDamge);
            //        //���� �Լ� �ҷ��´� �Ҹ�
            //    }
            //    else if (hit.collider.CompareTag("Barrel"))//�巳���ϰ��
            //    {
            //        //�巳�� ���߽�Ű�⵵ �־����
            //        hit.collider.gameObject.GetComponent<Barrel>().Hurt();
            //    }
            //}







        }






    }

}
