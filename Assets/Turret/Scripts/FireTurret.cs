using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTurret : Turret
{
    
    private Vector3 attackBoxPos;
    private int nowFireTurretUpgradeCount = 0;
    private int nowFireTurretHp;
    private int maxFireTurretHp = 8;
    private int fireTurretHpRise = 5;
    private float attackRangeX = 2;
    private float fireTurretMakingTime = 15;
    private float fireTurretAttackDamge = 5;
    private float fireTurretAttackSpeed = 5f;
    private float fireTurretAttackRange = 5;
    private float fireTurretUpgradeTime = 15;
    private float fireTurretRepairTime = 15;
    private float fireTurretAttackRise = 1.5f;
    private float fireTurretAttackSpeedRise = 0.5f;
    private float fireTurretUpgradCostRise = 2f;
    private float fireTurretMaxUpgradeCount = 5;
    private float fireTurretRepairCost = 5;
    private float fireTurretUpgradeCost = 10;
    private float fireTurretMakingCost = 15;
    private Vector3 attackBoxRange;
    protected override void Awake()
    {
        base.Awake();
        attackBoxRange = new Vector3(attackRangeX/2, 3, fireTurretAttackRange/2);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        base.SetTurret(fireTurretMakingTime, fireTurretMakingCost, fireTurretAttackDamge, fireTurretAttackSpeed, fireTurretAttackRange, maxFireTurretHp, fireTurretHpRise, fireTurretUpgradeCost, fireTurretUpgradeTime, fireTurretRepairTime, fireTurretRepairCost, fireTurretAttackRise, fireTurretAttackSpeedRise, fireTurretUpgradCostRise, fireTurretMaxUpgradeCount);
        
    }

    public override void Attack()
    {
        if(Physics.Raycast(firePos.transform.position,Vector3.forward,out RaycastHit hit, fireTurretAttackRange))
        {
            if (!hit.collider.CompareTag("Monster"))
            {
                return;
            }
            else
            {
                attackBoxPos = new Vector3(firePos.transform.position.x, firePos.transform.position.y, firePos.transform.position.z + fireTurretAttackRange);
                //이펙트 생성
                Collider[] colliders = Physics.OverlapBox(attackBoxPos, attackBoxRange);

                foreach (Collider collider in colliders)
                {
                    if (collider.CompareTag("Monster"))
                    {
                        //데미지 주는부분
                        Debug.Log("지속 터렛 공격");
                    }
                    //else if ()//드럼통 데미지 부분
                    //{

                    //}
                }
            }
        }
    }
}
