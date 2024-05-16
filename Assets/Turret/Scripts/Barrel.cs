using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Barrel : MonoBehaviour
{
    //private int nowUpgradeCount;
    //private int nowHp;
    //private int maxHp;
    //private int hpRise;
    //private float makingTime;
    //private float makingCost;
    //private float attackDamge;
    //private float attackSpeed;
    //private float attackRange;
    //private float upgradeCost;
    //private float upgradeTime;
    //private float repairTime;
    //private float repairCost;
    //private float attackRise;
    //private float attackSpeedRise;
    //private float upgradCostRise;
    //private float maxUpgradeCount;

    //private void Awake()
    //{
    //    base.SetTurret(makingTime,makingCost,attackDamge,attackRange,maxHp,hpRise,upgradeCost,upgradeTime,)
    //}

    [SerializeField]
    private GameObject makingEffect;
    [SerializeField]
    private GameObject explosionEffect;

    private int attackDamge = 300;
    //private int hp = 1;
    private int range = 5;
    private LayerMask monsterLayer;
    private LayerMask turretLayer;
    private MeshRenderer meshRenderer;
    private float checkTime;
    private float makingTime = 15 * 60;

    private BoxCollider bodyColleder;

    private bool isMakeFinsh;


    public bool isMake;

    public int makingCost = 5;

    //현재 태그와 레이어 설정안함 0512기준

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        bodyColleder = GetComponent<BoxCollider>();
        turretLayer = LayerMask.NameToLayer("Turret");
        
    }

    private void OnEnable()
    {
        meshRenderer.enabled = true;
        bodyColleder.enabled = false;
        isMakeFinsh = false;
        gameObject.layer = LayerMask.NameToLayer("debug");
        explosionEffect.SetActive(false);
        makingEffect.SetActive(false);
    }

    private void Update()
    {
        if (!isMakeFinsh)
        {
            MakeCheck();

        }
    }

    private void MakeCheck()
    {
        Collider[] turretColliders = Physics.OverlapSphere(transform.position, 5, (1 << turretLayer));


        if (turretColliders.Length > 0)
        {
            isMake = false;
            meshRenderer.material.color = Color.red;
        }
        else
        {

            isMake = true;
            meshRenderer.material.color = Color.blue;

        }
    }
    public void Making()
    {
        meshRenderer.enabled = false;
        //이펙트 재생
        makingEffect.SetActive(true);
        while (true)
        {
            checkTime += Time.time;
            if(checkTime >= makingTime)
            {
                meshRenderer.enabled = true;
                meshRenderer.material.color = Color.white;
                bodyColleder.enabled = true;
                gameObject.tag = "Barrel";
                gameObject.layer = LayerMask.NameToLayer("Turret");
                checkTime = 0;
                isMakeFinsh = true;
                break;
            }
        }
        makingEffect.SetActive(false);
    }

    public void Hurt()
    {

        Collider[] enemyCollider = Physics.OverlapSphere(transform.position, range, (1 << monsterLayer));

        meshRenderer.enabled = false;
        //이펙트 생성
        explosionEffect.SetActive(true);

        if (enemyCollider.Length > 0)
        {
            foreach (Collider collider in enemyCollider)
            {
                //몬스터의 hurt함수나 어쩃든 데미지 주는기능
            }
        }

        gameObject.tag = "Untagged";
        gameObject.layer = LayerMask.NameToLayer("debug");

        gameObject.transform.parent.gameObject.SetActive(false);
    }

    //private void OnDisable()
    //{
    //    MultiObjectPool.ReturnToPool(gameObject);
    //}

}
