using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTurret : Turret
{
    private Vector3 attackBoxRange;
    private Vector3 attackBoxPos;
    private int nowUpgradeCount = 0;
    private int nowHp;
    private int maxHp = 8;
    private int hpRise = 5;
    private float attackRangeX = 2;
    private float makingTime = 15;
    private float attackDamge = 5;
    private float attackSpeed = 5f;
    private float attackRange = 5;
    private float upgradeTime = 15;
    private float repairTime = 15;
    private float attackRise = 1.5f;
    private float attackSpeedRise = 0.5f;
    private float upgradCostRise = 2f;
    private float maxUpgradeCount = 5;
    private float repairCost = 5;
    private float upgradeCost = 10;
    private float makingCost = 15;

    private void Awake()
    {
        attackBoxRange = new Vector3(attackRangeX/2, 3, attackRange/2);
    }

    private void OnEnable()
    {
        SetTurret(makingTime, makingCost, attackDamge, attackSpeed, attackRange, maxHp, hpRise, upgradeCost, upgradeTime, repairTime, repairCost, attackRise, attackSpeedRise, upgradCostRise, maxUpgradeCount);
        attackBoxPos = new Vector3(firePos.transform.position.x, firePos.transform.position.y, firePos.transform.position.z + attackRange);
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
                //이펙트 생성
                Collider[] colliders = Physics.OverlapBox(attackBoxPos, attackBoxRange);

                foreach (Collider collider in colliders)
                {
                    if (collider.CompareTag("Monster"))
                    {
                        //데미지 주는부분
                    }
                    //else if ()//드럼통 데미지 부분
                    //{

                    //}
                }
            }
        }
    }
}
