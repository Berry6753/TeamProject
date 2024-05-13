using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleTurret : Turret
{

    private int nowUpgradeCount = 0;
    private int nowHp;
    private int maxHp = 5;
    private int hpRise = 2;
    private float attackRadius = 3;
    private float nowAttackRadius;
    private float attackRadiusRise = 0.5f;
    private float makingTime = 15;
    private float attackDamge = 50;
    private float attackSpeed = 0.5f;
    private float attackRange = 15;
    private float upgradeTime = 15;
    private float repairTime = 15;
    private float attackRise = 20;
    private float attackSpeedRise = 0.3f;
    private float upgradCostRise = 1.3f;
    private float maxUpgradeCount = 4;
    private float repairCost = 8;
    private float upgradeCost = 15;
    private float makingCost = 20;

    private void OnEnable()
    {
        SetTurret(makingTime, makingCost, attackDamge, attackSpeed, attackRange, maxHp, hpRise, upgradeCost, upgradeTime, repairTime, repairCost, attackRise, attackSpeedRise, upgradCostRise, maxUpgradeCount);
        nowAttackRadius = attackRadius;
    }

    protected override void Attack()
    {
        if(Physics.Raycast(firePos.transform.position,Vector3.forward,out RaycastHit hit, attackRange))
        {
            if (!hit.collider.CompareTag("Monster"))
            {
                return;
            }
            else
            {
                Collider[] targets = Physics.OverlapSphere(targetTransform.position, nowAttackRadius, monsterLayer);
                //이펙트 생성
                foreach (Collider target in targets)
                {
                    //몬스터 데미지주는 부분
                    //드럼통 터지는 부분
                }
            }
        }
        

    }

    public override void Upgrade()
    {
        base.Upgrade();
        nowAttackRadius += attackRadiusRise;
    }

}
