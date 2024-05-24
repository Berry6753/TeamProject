using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleTurret : Turret
{
    private int nowMissleTurretUpgradeCount = 0;
    private int nowMissleTurretHp;
    private int maxMissleTurretHp = 5;
    private int missleTurretHpRise = 2;
    private float missleTurretAttackRadius = 3;
    private float missleTurretNowAttackRadius;
    private float missleTurretAttackRadiusRise = 0.5f;
    private float missleTurretMakingTime = 15;
    private float missleTurretAttackDamge = 50;
    private float missleTurretAttackSpeed = 0.5f;
    private float missleTurretAttackRange = 25;
    private float missleTurretUpgradeTime = 15;
    private float missleTurretRepairTime = 15;
    private float missleTurretAttackRise = 20;
    private float missleTurretAttackSpeedRise = 0.3f;
    private float missleTurretUpgradCostRise = 1.3f;
    private float missleTurretMaxUpgradeCount = 4;
    private float missleTurretRepairCost = 8;
    private float missleTurretUpgradeCost = 15;
    private float missleTurretMakingCost = 20;
    

    [SerializeField]
    private GameObject explosionEffect;
    private AudioSource explosionAudio;
    private ParticleSystem explosionPaticle;
    private LayerMask barrelLayer;

    protected override void Awake()
    {
        base.Awake();
        explosionAudio = explosionEffect.GetComponent<AudioSource>();
        explosionPaticle = explosionEffect.GetComponent<ParticleSystem>();
        barrelLayer = LayerMask.NameToLayer("Turret");
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        base.SetTurret(missleTurretMakingTime, missleTurretMakingCost, missleTurretAttackDamge, missleTurretAttackSpeed, missleTurretAttackRange, maxMissleTurretHp, missleTurretHpRise, 
            missleTurretUpgradeCost, missleTurretUpgradeTime, missleTurretRepairTime, missleTurretRepairCost, missleTurretAttackRise, missleTurretAttackSpeedRise, missleTurretUpgradCostRise, missleTurretMaxUpgradeCount);
        missleTurretNowAttackRadius = missleTurretAttackRadius;
        explosionEffect.SetActive(false);
    }

    public override void Attack()
    {
        if (targetIndex >= turretTargetList.Count)
        {
            return;
        }

        targetTransform = targetList[targetIndex].transform;

        if (Physics.Raycast(firePos.transform.position, targetTransform.position - firePos.transform.position,out RaycastHit hit, missleTurretAttackRange,~(ignoreLayer)))
        {
            if (!hit.collider.CompareTag("Monster"))
            {
                targetIndex++;
                return;
            }
            else
            {
                Collider[] targets = Physics.OverlapSphere(targetTransform.position, missleTurretNowAttackRadius, (1 << monsterLayer) | (1 << barrelLayer));
                //����Ʈ ����
                fireEfect.SetActive(true);
                explosionEffect.SetActive(true);
                explosionEffect.transform.position = targetTransform.gameObject.transform.position;
                explosionPaticle.Play();
                explosionAudio.Play();
                firePaticle.Play();
                fireAudio.pitch = Time.timeScale;
                fireAudio.Play();
                foreach (Collider target in targets)
                {
                    if (target.CompareTag("Monster"))
                    {
                        //���� �������ִ� �κ�
                        target.gameObject.GetComponent<Monster>().Hurt(base.turretAttackDamge);
                        Debug.Log("���� �ͷ� ����");

                    }
                    //�巳�� ������ �κ�
                    if (target.CompareTag("Barrel"))//�巳���ϰ��
                    {
                        //�巳�� ���߽�Ű�⵵ �־����
                        target.gameObject.GetComponent<Barrel>().Hurt();
                    }
                }
            }
            
        }

        
        

    }

    public override void Upgrade()
    {
        if (base.turretUpgradeCount < 3)
            fireEffectPos.transform.localPosition = fireEffectPos.transform.localPosition + new Vector3(0.03f, 0f, 0);
        else
            fireEffectPos.transform.localPosition = fireEffectPos.transform.localPosition + new Vector3(0.02f, 0f, 0);

        base.Upgrade();
        missleTurretNowAttackRadius += missleTurretAttackRadiusRise;

        if (base.turretUpgradeCount == 2)
            fireEffectPos.transform.localPosition = fireEffectPos.transform.localPosition + new Vector3(0f, 0f, -0.03f);
    }

}
