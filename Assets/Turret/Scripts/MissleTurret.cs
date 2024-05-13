using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleTurret : Turret
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

    private void Awake()
    {
        SetTurret(makingTime, makingCost, attackDamge, attackSpeed, attackRange, maxHp, hpRise, upgradeCost, upgradeTime, repairTime, repairCost, attackRise, attackSpeedRise, upgradCostRise, maxUpgradeCount);
    }

    protected override void Attack()
    {
       
    }

}
