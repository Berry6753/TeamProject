using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;

public abstract class Monster : MonoBehaviour
{
    [Header("Monster")]
    [SerializeField] protected string monsterName;

    public string GetMonsterName {  get { return monsterName; } }

    [Header("몬스터 스탯")]
    protected float time = 0;
    [SerializeField] protected float attackSpeed;       //공격 속도
    [SerializeField] protected float hp;                //체력
    [SerializeField] protected float damage;            //공격력
    [SerializeField] protected float hitNum;            //타격 횟수
    [SerializeField] protected float attackRange;       //사거리
    [Header("스탯 성장치")]
    [SerializeField] protected float upScaleHp;         //체력 성장치
    [SerializeField] protected float upScaleDamage;     //공격력 성장치
    [Header("스폰 관련")]
    [SerializeField] public int startSpawnNum;        //초기 스폰 수
    [SerializeField] public int upScaleSpwanNum;      //스폰 증가 수 
    [SerializeField] protected int spawnTiming;       //스폰 기점

    [SerializeField] protected float sensingRange;      //감지 범위
    protected int turretIndex = 0;                      //가장 가까운 터렛인덱스
    protected int secondTurretIndex = 0;                //두 번째 가까운 터렛인덱스
    protected int targetingIndex = 0;                   //타겟으로 삼을 터렛인덱스
    protected Collider[] attack;                          //공격 콜라이더

    protected Transform monsterTr;                      //몬스터 위치
    protected Transform defaultTarget;                  //기본 타겟
    protected Transform chaseTarget;
    [SerializeField] protected LayerMask turretLayer;   //터렛레이어
    [SerializeField] protected LayerMask monsterLayer;  //몬스터레이어
    [SerializeField] protected LayerMask dieLayer;

    protected int probabilityGetGear;
    protected int probabilityNum;
    public int dropGearNum = 0;
    protected int _wave = 0;
    [HideInInspector]
    public int wave
    { 
        get { return _wave; }
        set 
        {
            if (_wave != value)
            { 
                _wave = value;
                UpScaleHp();
                UpScaleDamage();
            }  
        }
        
    }

    
    protected Rigidbody rb;
    protected NavMeshAgent nav;
    protected Animator anim;
    protected StateMachine stateMachine;

    protected readonly int hashTrace = Animator.StringToHash("isTrace");
    protected readonly int hashAttack = Animator.StringToHash("isAttack");
    protected readonly int hashDie = Animator.StringToHash("isDie");

    public enum State
    { IDLE, TRACE, ATTACK, DIE}
    public State state = State.IDLE;

    protected bool canAttack = true;
    [HideInInspector] public bool isDead = false;
    //생존 여부

    public event Action<Monster> OnDeath;
    protected WaveSystem waveSystem;
 
    protected virtual void Awake()
    {
        monsterTr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        attack = GetComponentsInChildren<SphereCollider>();
        foreach (Collider c in attack)
        {
            c.enabled = false;
        }
        GameObject waveObject = GameObject.Find("WaveSystem");
        waveSystem = waveObject.GetComponent<WaveSystem>();

        stateMachine = gameObject.AddComponent<StateMachine>();

        stateMachine.AddState(State.IDLE, new IdleState(this));
        stateMachine.AddState(State.TRACE, new TraceState(this));
        stateMachine.AddState(State.ATTACK, new AttackState(this));
        stateMachine.AddState(State.DIE, new DieState(this));
        stateMachine.InitState(State.IDLE);
    }

    protected void OnEnable()
    {
        gameObject.layer = monsterLayer;
    }

    protected virtual void Update()
    {
        if (time > 0 && !anim.GetBool("isAttack"))
        {
            time -= Time.deltaTime;
            canAttack = false;
        }
        else if(time <= 0)
        {
            canAttack = true;
        }
        wave = waveSystem.currentWaveIndex - 1;
    }

    protected virtual void LookAt()
    {
        transform.LookAt(new Vector3(chaseTarget.position.x, transform.position.y, chaseTarget.position.z));
    }

    protected abstract void ChaseTarget();              //타겟 추적

    protected virtual IEnumerator MonsterState()        //몬스터 행동 설정
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(0.3f);
            if ( hp <= 0 /*state == State.DIE*/)
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
                    foreach (Collider c in attack)
                    {
                        c.enabled = true;
                    }
                }
                else
                {
                    stateMachine.ChangeState(State.IDLE);
                    foreach (Collider c in attack)
                    {
                        c.enabled = false;
                    }
                }
            }
            else
            {
                stateMachine.ChangeState(State.TRACE);
                foreach (Collider c in attack)
                {
                    c.enabled = false;
                }
            }
        }
    }

    protected void AttackEnd()
    {
        anim.SetBool("isAttack", false);
    }

    protected void AttackDelay()
    {
        time = attackSpeed;       
    }

    protected void PriorityTarget()                     //타겟 우선순위 설정
    {
        Collider[] turret = Physics.OverlapSphere(transform.position, sensingRange, turretLayer);
        if (turret.Length > 0)
        {   
            turretDistance(turret);
            TargetingTurret();
            if (targetingIndex != -1)
            {
                chaseTarget = turret[targetingIndex].transform;
            }
            else if (targetingIndex == -1)
            {
                chaseTarget = defaultTarget;
            }
        }
        else
        {
            chaseTarget = defaultTarget;
        }
    }

    protected void turretDistance(Collider[] array)
    {
        float[] distance = new float[array.Length];
        int minIndex = 0;
        int secondMinIndex = -1;
        for (int i = 0; i < array.Length; i++)
        {
            distance[i] = Vector3.Distance(array[i].transform.position, monsterTr.position);
        }
        float minDistance = distance[0];
        float secondDistance = distance.Max();
        for (int i = 0; i < distance.Length; i++)
        {
            if (distance[i] < minDistance)
            {
                secondDistance = minDistance;
                secondMinIndex = minIndex;
                minDistance = distance[i];
                minIndex = i;
            }
            else if(distance[i] < secondDistance)
            {
                secondDistance = distance[i];
                secondMinIndex = i;
            }
        }
        turretIndex = minIndex;
        secondTurretIndex = secondMinIndex;
    }

    protected void TargetingTurret()
    {
        Collider[] monster = Physics.OverlapBox(transform.position + Vector3.forward * 2.5f, new Vector3(5f, 5f, 5f), transform.rotation, monsterLayer);
        if (monster.Length > 3)
        {
            targetingIndex = secondTurretIndex;
        }
        else
        {
            targetingIndex = turretIndex;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + Vector3.forward * 2.5f, new Vector3(5f, 5f, 5f));
    }

    protected void FreezeVelocity()                     //물리력 제거
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    //protected abstract void SpawnTiming();              //스폰 타이밍

    protected void UpScaleHp()                          //체력 증가량
    {
        hp += upScaleHp * wave;
    }

    protected virtual void UpScaleDamage()              //데미지 증가량
    {
        damage += upScaleDamage * wave;
    }

    public void Hurt(float damage)                   //플레이어에게 데미지 입을 시
    { 
        hp -= damage;
    }

    public virtual void isDie()                              //죽었을 시
    { 
        isDead = true;
        RandomGear();
        gameObject.layer = dieLayer;
        OnDeath?.Invoke(this);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        MonsterObjectPool.ReturnToPool(gameObject);
    }

    protected void RandomGear()
    {
        probabilityGetGear = UnityEngine.Random.Range(0, 100);
        if (probabilityGetGear >= 0 && probabilityGetGear < 65)
        {
            probabilityNum = UnityEngine.Random.Range(0, 100);
            if (probabilityNum >= 0 && probabilityNum < 60)
            {
                dropGearNum = 1;
            }
            else if (probabilityNum >= 60 && probabilityNum < 80)
            {
                dropGearNum = 2;
            }
            else if (probabilityNum >= 80 && probabilityNum < 92)
            {
                dropGearNum = 3;
            }
            else if (probabilityNum >= 92 && probabilityNum < 97)
            {
                dropGearNum = 4;
            }
            else
            {
                dropGearNum = 5;
            }
        }
        else
        {
            dropGearNum = 0;
        }
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            FreezeVelocity();
            ChaseTarget();
        }
    }
    protected void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            FreezeVelocity();
            ChaseTarget();
        }
    }

    protected class BaseMonsterState : BaseState
    {
        protected Monster owner;
        public BaseMonsterState(Monster owner)
        { this.owner = owner; }
    }
    protected class IdleState : BaseMonsterState
    {
        public IdleState(Monster owner) : base(owner) { }
        public override void Enter()
        {
            owner.nav.isStopped = true;
            owner.anim.SetBool(owner.hashTrace, false);
        }
    }
    protected class TraceState : BaseMonsterState
    {
        public TraceState(Monster owner) : base(owner) { }
        public override void Enter()
        {
            owner.nav.SetDestination(owner.chaseTarget.position);
            owner.nav.isStopped = false;
            owner.anim.SetBool(owner.hashTrace, true);
            owner.anim.SetBool(owner.hashAttack, false);
        }
    }
    protected class AttackState : BaseMonsterState
    {
        public AttackState(Monster owner) : base(owner) { }
        public override void Enter()
        {
            owner.anim.SetBool(owner.hashAttack, true);
        }
    }
    protected class DieState : BaseMonsterState
    {
        public DieState(Monster owner) : base(owner) { }
        public override void Enter()
        {
            owner.anim.SetTrigger(owner.hashDie);
        }
    }
}