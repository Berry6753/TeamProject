using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGunTurret : Turret
{

    private int nowMiniGunUpgradeCount = 0;
    private int nowMiniGunHp;
    private int maxMiniGunHp = 5;
    private int miniGunhpRise = 3;
    private float miniGunMakingTime = 15;
    private float miniGunAttackDamge = 25;
    private float miniGunAttackSpeed = 2;
    private float miniGunAttackRange = 23;
    private float miniGunUpgradeTime = 15;
    private float miniGunRepairTime = 15;
    private float miniGunAttackRise = 5;
    private float miniGunAttackSpeedRise = 0.5f;
    private float miniGunUpgradCostRise = 2;
    private float miniGunMaxUpgradeCount = 5;
    private float miniGunRepairCost = 5;
    private float miniGunUpgradeCost = 10;
    private float miniGunMakingCost = 15;
    

    protected override void OnEnable()
    {
        base.OnEnable();
        base.SetTurret(miniGunMakingTime, miniGunMakingCost, miniGunAttackDamge, miniGunAttackSpeed, miniGunAttackRange, maxMiniGunHp, miniGunhpRise, miniGunUpgradeCost
            , miniGunUpgradeTime, miniGunRepairTime, miniGunRepairCost, miniGunAttackRise, miniGunAttackSpeedRise, miniGunUpgradCostRise, miniGunMaxUpgradeCount);
    }

    public override void Attack()
    {
        if (targetIndex >= turretTargetList.Count)
        {
            return;
        }

        targetTransform = targetList[targetIndex].transform;

        if (Physics.Raycast(firePos.transform.position, targetTransform.position - firePos.transform.position, out RaycastHit hit, miniGunAttackRange,~(ignoreLayer)))
        {
            
            if (!hit.collider.CompareTag("Monster"))
            {
                targetIndex++;
                return;
            }
            else if (hit.collider.CompareTag("Monster"))
            {
                fireEfect.SetActive(true);
                if (!fireAudio.isPlaying)
                {
                    fireAudio.Play();
                }
                //���� ������ �ִ� �κ�
                //���� �Լ� �ҷ��´� �Ҹ�
                hit.collider.gameObject.GetComponent<Monster>().Hurt(base.turretAttackDamge);
                Debug.Log("���� �ͷ� ����");

            }
            else if (hit.collider.CompareTag("Barrel"))
            {
                //�巳�� ���߽�Ű�⵵ �־����
                hit.collider.gameObject.GetComponent<Barrel>().Hurt();
            }
            
        }

    }

    public override void Upgrade()
    {
        if (base.turretUpgradeCount < 1) 
        fireEffectPos.transform.localPosition = fireEffectPos.transform.localPosition+ new Vector3(0, -0.001f, 0);
        base.Upgrade();
    }

}
