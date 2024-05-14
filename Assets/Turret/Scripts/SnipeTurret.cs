using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnipeTurret : Turret
{
    private int nowSnipeTurretUpgradeCount = 0;
    private int nowSnipeTurretHp;
    private int maxSnipeTurretHp = 5;
    private int snipeTurretHpRise = 1;
    private float snipeTurretMakingTime = 15;
    private float snipeTurretAttackDamge = 100;
    private float snipeTurretAttackSpeed = 0.35f;
    private float snipeTurretAttackRange = 20;
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
    }

    public override void Attack()
    {
        if(Physics.Raycast(firePos.transform.position,Vector3.forward,out RaycastHit hit, snipeTurretAttackRange))
        {
            if (!hit.collider.CompareTag("Monster"))
            {
                return;
            }
            else
            {
                RaycastHit[] raycastHits = Physics.RaycastAll(firePos.transform.position, Vector3.forward, snipeTurretAttackRange);

                foreach (RaycastHit monster in raycastHits)
                {

                    //if(/*벽이나 터렛에 부딫힐때*/)
                    //{ 
                    //    //벽이나 터렛에 부딫힌 이펙트
                    //    return;
                    //
                    //}
                    //else 
                    if (hit.collider.CompareTag("Monster"))
                    {
                        Debug.Log("관통 터렛 공격");
                        //이펙트 생성
                        //몬스터 데미지 주는 부분
                        //몬스터 함수 불러온단 소리
                    }
                    //else if(hit.collider.CompareTag(""))//드럼통일경우
                    //{
                    //    //드럼통 폭발시키기도 있어야함
                    //    hit.collider.gameObject.GetComponent<Barrel>().Hurt();
                    //}
                }
            }
        }
        
        
    }

}
