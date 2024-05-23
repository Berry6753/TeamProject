using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AerialMonster : Monster
{
    [SerializeField] private ParticleSystem fireball;

    protected override void Awake()
    {
        base.Awake();
        defaultTarget = GameObject.FindWithTag("Core").GetComponent<Transform>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
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
            if (hp <= 0)
            {
                stateMachine.ChangeState(State.DIE);
                isDie();
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

    private void FreezeVelocity()                     //물리력 제거
    {
        nav.isStopped = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public override void isDie()
    { 
        base.isDie();
        nav.areaMask = 1 << NavMesh.GetAreaFromName("Walkable");
    }

    private void PlayFireball()
    { 
        fireball.Play();
    }
}
