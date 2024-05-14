using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerialMonster : Monster
{
    protected override void Awake()
    {
        base.Awake();
        defaltTarget = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }
    private void Start()
    {
        ChaseTarget();
    }
    protected override void Update()
    {
        base.Update();
        PriorityTarget();
        transform.LookAt(chaseTarget);
        Debug.Log(state);
    }

    protected override void ChaseTarget()
    {
        StartCoroutine(MonsterState());
    }

    protected override IEnumerator MonsterState()        //몬스터 행동 설정
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(0.3f);
            if (state == State.DIE)
            {
                stateMachine.ChangeState(State.DIE);
                yield break;
            }

            float distance = Vector3.Distance(chaseTarget.position, monsterTr.position);
            if (distance <= attackRange)
            {
                FreezeVelocity();
                if (canAttack)
                {
                    stateMachine.ChangeState(State.ATTACK);
                }
                else
                {
                    stateMachine.ChangeState(State.IDLE);
                }
            }
            else
            {
                stateMachine.ChangeState(State.TRACE);
            }
        }
    }

    protected override void SpawnTiming()
    {
        throw new System.NotImplementedException();
    }
}
