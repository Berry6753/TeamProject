using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : MonoBehaviour
{
    private float searchTime = 0.5f;
    private float checkSearchTime;

    protected Transform targetTransform;
    protected LayerMask monsterLayer = 6;

    protected float makingTime;
    protected float makingCost;
    protected float attackDamge;
    protected float attackSpeed;
    protected float attackRange;
    protected float hp;
    protected float upgradeCost;
    protected float upgradeTime;
    protected float repairTime;
    protected float repairCost;
    protected float attackRise;
    protected float hpRise;
    protected float attackSpeedRise;
    protected float upgradCostRise;
    protected float maxUpgradeCount;

    protected bool isUpgrade;
    protected bool isRepair;
    protected bool isTarget;

    protected abstract void Attack();

    //코루틴은 가비지컬렉터가 많이 불린다
    //메모리를 많이 먹는다는 뜻이다
    //포탑이 많아질 예정이니 코루틴 없이 구현해보자
    //하지만 함수를 업데이트에서 호출해 비교하면서 하는것보단 좋다
    //공격은 이벤트를 이용해 만들어 보자
    protected IEnumerator SearchEnemy()
    {
        while (true)
        {
            yield return new WaitUntil(() => targetTransform == null);
            yield return new WaitForSeconds(1);

            Collider[] enemyCollider = Physics.OverlapSphere(transform.position, attackRange, monsterLayer);//레이어 마스크 몬스터 추가
            Transform nierTargetTransform = null;
            if (enemyCollider.Length > 0)
            {
                float nierTargetDistance = Mathf.Infinity;
                foreach (Collider collider in enemyCollider)
                {
                    float distance = Vector3.SqrMagnitude(transform.position - collider.transform.position);

                    if (distance < nierTargetDistance)
                    {
                        nierTargetDistance = distance;
                        nierTargetTransform = collider.transform;
                    }
                }
            }

            targetTransform = nierTargetTransform;

        }

    }


}
