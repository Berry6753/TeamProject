using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class Monster : MonoBehaviour
{
    [Header("몬스터 스탯")]
    [SerializeField] protected string monsterName;      //몬스터 종류
    [SerializeField] protected float attactSpeed;       //공격 속도
    [SerializeField] protected float hp;                //체력
    [SerializeField] protected float damage;            //공격력
    [SerializeField] protected float hitNum;            //타격 횟수
    [Header("스탯 성장치")]
    [SerializeField] protected float upScaleHp;         //체력 성장치
    [SerializeField] protected float upScaleDamage;     //공격력 성장치
    [Header("스폰 관련")]
    [SerializeField] protected float startSpwanNum;     //초기 스폰 수
    [SerializeField] protected float upScaleSpwanNum;   //스폰 증가 수 
    [SerializeField] protected float spawnTiming;       //스폰 기점

    protected float distance;                           //플레이어와의 거리 
    
    //[SerializeField] protected float sensingRange;    //감지 범위

    [SerializeField] protected Transform defaltTarget;  //기본 타겟
    protected List<Transform> turretTarget = new List<Transform>();       //터렛 타겟
    
    protected int wave;                   
    
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
        if (turretTarget != null)
        {
            for (int i = 0; i < turretTarget.Count; i++)
            {
                nav.SetDestination(turretTarget[i].transform.position);
                break;
            }
        }
        else if (turretTarget == null)
        {
            nav.SetDestination(defaltTarget.position);
        }
    }

    protected void FreezeVelocity()                     //물리력 제거
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    protected void UpScaleHp()
    {
        hp += upScaleHp * wave;
    }

    protected void UpScaleDamage()
    {
        damage += upScaleDamage * wave;
    }

    protected void UpScaleSpawn()
    {
        if (wave % 10 == 0 && wave / 10 > 0)
        {
            startSpwanNum += upScaleSpwanNum;
        }
    }

    protected void Hurt(float damage)                   //플레이어에게 데미지 입을 시
    { 
        hp -= damage;
    }

    protected void isDie()                              //죽었을 시
    { 
        isDead = true;
        Destroy(this.gameObject);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Turret"))
        {
            turretTarget.Add(other.gameObject.transform);
            isChase = true;
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Turret"))
        {
            turretTarget.Remove(other.gameObject.transform);
            isChase = false;
        }
    }
}