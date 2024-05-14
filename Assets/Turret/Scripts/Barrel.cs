using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private int attackDamge = 300;
    //private int hp = 1;
    private int range = 5;
    private LayerMask monsterLayer;
    private MeshRenderer meshRenderer;
    private float checkTime;
    private float makingTime = 15 * 60;

    private SphereCollider checkColleder;
    private BoxCollider bodyColleder;

    public bool isMake;

    public int makingCost = 5;

    //현재 태그와 레이어 설정안함 0512기준

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        checkColleder = GetComponent<SphereCollider>();
        bodyColleder = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        meshRenderer.enabled = true;
        checkColleder.enabled = true;
        bodyColleder.enabled = false;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Turret"))
        {
            meshRenderer.material.color = Color.red;
            isMake = false;
        }
        else
        {
            meshRenderer.material.color = Color.blue;
            isMake = true;
        }
    }

    public void Making()
    {
        meshRenderer.enabled = false;
        //이펙트 재생
        while (true)
        {
            checkTime += Time.time;
            if(checkTime >= makingTime)
            {
                meshRenderer.enabled = true;
                meshRenderer.material.color = Color.white;
                checkColleder.enabled = false;
                bodyColleder.enabled = true;
                gameObject.tag = "Barrel";
                gameObject.layer = 8;
                checkTime = 0;
                break;
            }
        }
    }

    public void Hurt()
    {

        Collider[] enemyCollider = Physics.OverlapSphere(transform.position, range, monsterLayer);

        meshRenderer.enabled = false;
        //이펙트 생성


        if (enemyCollider.Length > 0)
        {
            foreach (Collider collider in enemyCollider)
            {
                //몬스터의 hurt함수나 어쩃든 데미지 주는기능
            }
        }

        gameObject.tag = "Untagged";
        gameObject.layer = 0;

        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
