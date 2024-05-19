using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerialMonster : Monster
{
    [SerializeField] private ParticleSystem fireball;

    protected override void Awake()
    {
        base.Awake();
        defaultTarget = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }
    private void Start()
    {
        ChaseTarget();
    }
    protected override void Update()
    {
        base.Update();
        PriorityTarget();
        LookAt();
    }

    protected override void LookAt()
    {
        transform.LookAt(chaseTarget);
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

    private void PlayFireball()
    { 
        fireball.Play();
    }
}
