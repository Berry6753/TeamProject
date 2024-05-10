using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Monster : MonoBehaviour
{
    [SerializeField] protected string monsterName;      //몬스터 종류
    [SerializeField] protected float attactSpeed;       //공격 속도
    [SerializeField] protected float hp;                //체력
    [SerializeField] protected float damage;            //공격력
    [SerializeField] protected float speed;             //이동 속도
    [SerializeField] protected float hitNum;            //타격 횟수
    [SerializeField] protected float upScaleHp;         //체력 성장치
    [SerializeField] protected float upScaleDamage;     //공격력 성장치
    [SerializeField] protected float startSpwanNum;     //초기 스폰 수
    [SerializeField] protected float upScaleSpwanNum;   //스폰 증가 수 
    [SerializeField] protected float spawnTiming;       //스폰 기점
    [SerializeField] protected float sensingRange;      //감지 범위

    [SerializeField] protected Transform[] target;      //추적할 타겟
    
    protected Rigidbody rb;
    protected NavMeshAgent nav;

    protected bool isDead = false;                      //생존 여부
    protected bool isChase = false;

    protected void Start()
    {
        rb = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
    }

    protected abstract void ChaseTarget();              //타겟 추적

    protected void PriorityTarget()                     //타겟 우선순위 추적
    {
        if (isChase)
        {
            for (int i = 0; i < target.Length; i++)
            {
                if (target[i] != null)
                {
                    nav.SetDestination(target[i].position);
                    break;
                }
            }
        }
    }

    protected void FreezeVelocity()                     //물리력 제거
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    protected void Hurt(float damage)                   //플레이어에게 데미지 입을 시
    { 
        hp -= damage;
    }

    protected void isDie()                              //죽었을 시
    { 
        isDead = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Turret"))
        { 
            isChase = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Turret"))
        { 
            isChase = false;
        }
    }
}
