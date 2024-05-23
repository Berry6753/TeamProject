using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AerialMonster : Monster
{
    [SerializeField] private ParticleSystem fireball;
    [SerializeField] private GameObject firePos;
    private LayerMask ignoreLayer;


    protected override void Awake()
    {
        base.Awake();
        ignoreLayer = 1 << LayerMask.NameToLayer("CheckZone") | 1 << LayerMask.NameToLayer("Ignore Raycast") | 1 << LayerMask.NameToLayer("Item")|1 << LayerMask.NameToLayer("Monster");
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
                Debug.Log("AAAAAAAAAAAAAAAAAAA");
                Physics.Raycast(firePos.transform.position, chaseTarget.position - firePos.transform.position, out RaycastHit hit, attackRange, ~(ignoreLayer));
                Debug.DrawRay(monsterTr.position, (chaseTarget.position - monsterTr.position) * 100f, Color.red);
                Debug.Log(hit.collider.name+"AAAAAAAAAAAAAAAAAA");
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Turret")||hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    Debug.Log("BBBBBBBBBBBBBBBBBBBBBBBBBBB");
                    FreezeVelocity();
                    if (canAttack)
                    {
                        Debug.Log("CCCCCCCCCCCCCCCCCCCCCCCCCCCCC");
                        stateMachine.ChangeState(State.ATTACK);
                    }
                    else
                    {
                        Debug.Log("DDDDDDDDDDDDDDDDDDDDDDD");
                        stateMachine.ChangeState(State.IDLE);
                    }
                }
                else
                {
                    Debug.Log("EEEEEEEEEEEEEEEEEEEEEEEEE");
                    Debug.Log(hit.collider.gameObject.layer+"EEEEEEEEEEEEEEEEEEE");
                    stateMachine.ChangeState(State.TRACE);
                }
                
            }
            else
            {
                Debug.Log("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");
                stateMachine.ChangeState(State.TRACE);
            }
        }
    }

    protected override void FreezeVelocity()                     //물리력 제거
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
