using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{

    public int repairCost = 100;
    public int upgradeCost = 50;
    public bool isUpgrade = true;
    public bool isUpgrading = false;
    public bool isReloading = false;

    private int upgradeCoolTimeRise = 10;
    private int nowHp;
    private int maxHp = 100;
    private int hpRise = 30;
    private int realoadCoolTime = 30;
    private int upgradeCoolTime = 30;
    private int upgradeCostRise = 2;
    private int nowUpgradeCount;
    private int maxUpgradeCount = 5;
    private float checkReloadingTime;
    private float checkUpgradeTime;


    // Start is called before the first frame update
    void Start()
    {
        nowHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isReloading)
        {
            checkReloadingTime += Time.time;
            if (checkReloadingTime >= realoadCoolTime)
            {
                isReloading = true;
                checkReloadingTime = 0;
            }
        }

        if (isUpgrading)
        {
            checkUpgradeTime += Time.time;
            if (checkUpgradeTime >= upgradeCoolTime)
            {
                Upgrade();
                isUpgrading = false;
            }
        }

        if (nowUpgradeCount >= maxUpgradeCount)
        {
            isUpgrade = false;
        }

        //파괴될시 게임매니저의 코어가있음을 뜻하는 변수를 false로
    }

    public void Reloading()
    {
        //플레이어의 탄창수 채워줌

        isReloading = false;
    }

    public void Hurt(int damge)
    {
        nowHp -= damge;
    }

    public void Repair()
    {
        nowHp = maxHp;
    }

    public void Upgrade()
    {
        nowHp += hpRise;
        maxHp += hpRise;
        upgradeCost *= upgradeCostRise;
        upgradeCoolTime += upgradeCoolTimeRise;
        nowUpgradeCount++;
    }
}
