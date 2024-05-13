using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnipeTurret : Turret
{
    private int nowUpgradeCount = 0;
    private int nowHp;
    private int maxHp = 5;
    private int hpRise = 1;
    private float makingTime = 15;
    private float attackDamge = 100;
    private float attackSpeed = 0.35f;
    private float attackRange = 20;
    private float upgradeTime = 15;
    private float repairTime = 15;
    private float attackRise = 50;
    private float attackSpeedRise = 0.2f;
    private float upgradCostRise = 1.5f;
    private float maxUpgradeCount = 3;
    private float repairCost = 10;
    private float upgradeCost = 20;
    private float makingCost = 30;

    private void OnEnable()
    {
        SetTurret(makingTime, makingCost, attackDamge, attackSpeed, attackRange, maxHp, hpRise, upgradeCost, upgradeTime, repairTime, repairCost, attackRise, attackSpeedRise, upgradCostRise, maxUpgradeCount);
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
                RaycastHit[] raycastHits = Physics.RaycastAll(firePos.transform.position, Vector3.forward, attackRange);

                foreach (RaycastHit monster in raycastHits)
                {

                    //if(/*벽이나 터렛에 부딫힐때*/)
                    //{ 
                    //    //벽이나 터렛에 부딫힌 이펙트
                    //    return;
                    //
                    //}
                    //else if (hit.collider.CompareTag("Monster"))
                    //{
                    //    //이펙트 생성
                    //    //몬스터 데미지 주는 부분
                    //    //몬스터 함수 불러온단 소리
                    //}
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
