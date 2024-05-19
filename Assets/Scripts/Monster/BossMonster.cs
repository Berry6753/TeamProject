using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;

public class BossMonster : MonoBehaviour
{
    [Header("스탯")]
    [SerializeField] private float hp;                //체력
    [SerializeField] private float damage;            //공격력
    [SerializeField] private float hitNum;            //타격 횟수
    [SerializeField] private float attackRange;       //사거리
    [SerializeField] private float specialAttackRange;//특수 공격 사거리
    private float amongRange;
    [Header("스탯 성장치")]
    [SerializeField] protected float upScaleHp;         //체력 성장치

    private float dashSpeed;
    private float dashTime;
    private float time;

    private int wave = 0;
    private int lastWave = 0;

    private Transform defaultTarget;
    private Transform bossTr;
    private Transform chaseTarget;

    private Animator anim;
    private NavMeshAgent nav;
    private Rigidbody rb;
    private Collider mainCollider;
    private SkinnedMeshRenderer[] renderer;
    private SphereCollider[] attackC;
    private BoxCollider jumpAttackC;
    [SerializeField] private CapsuleCollider dashAttackC;

    [Header("")]
    [SerializeField] protected LayerMask turretLayer;   //터렛레이어
    [SerializeField] protected LayerMask monsterLayer;  //몬스터레이어
    [SerializeField] protected float sensingRange;      //감지 범위
    protected int turretIndex = 0;                      //가장 가까운 터렛인덱스
    protected int secondTurretIndex = 0;                //두 번째 가까운 터렛인덱스
    protected int targetingIndex = 0;                   //타겟으로 삼을 터렛인덱스

    private StateMachine stateMachine;

    private readonly int hashTrace = Animator.StringToHash("isTrace");
    private readonly int hashDie = Animator.StringToHash("isDie");
    private readonly int hashJumpA = Animator.StringToHash("isJumpA");
    private readonly int hashDashA = Animator.StringToHash("isDashA");
    private readonly int hashDefaultA = Animator.StringToHash("isDefaultA");
    private readonly int hashDefaultA2 = Animator.StringToHash("isDefaultA2");
    private readonly int hashJump = Animator.StringToHash("isJump");
    private readonly int hashDash = Animator.StringToHash("isDash");
    private readonly int hashAttack = Animator.StringToHash("isAttack");

    [HideInInspector] public bool isDead = false;
    private bool isDash = false;
    private bool canAttack = true;
    private bool canJump = true;
    private bool isBackward = false;

    public enum State
    { IDLE, TRACE, JumpA, DashA, DefaultA, DefaultA2, DIE }
    public State state = State.IDLE;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        mainCollider = GetComponent<Collider>();
        renderer = GetComponentsInChildren<SkinnedMeshRenderer>();
        bossTr = GetComponent<Transform>();
        defaultTarget = GameObject.FindWithTag("Player").GetComponent<Transform>();
        attackC = GetComponentsInChildren<SphereCollider>();
        jumpAttackC = GetComponentInChildren<BoxCollider>();
        stateMachine = gameObject.AddComponent<StateMachine>();

        stateMachine.AddState(State.IDLE, new IdleState(this));
        stateMachine.AddState(State.TRACE, new TraceState(this));
        stateMachine.AddState(State.JumpA, new JumpAState(this));
        stateMachine.AddState(State.DashA, new DashAState(this));
        stateMachine.AddState(State.DefaultA, new DefaultAState(this));
        stateMachine.AddState(State.DefaultA2, new DefaultA2State(this));
        stateMachine.AddState(State.DIE, new DieState(this));
        stateMachine.InitState(State.IDLE);

        amongRange = (attackRange + specialAttackRange) / 2;
    }

    private void Start()
    {
        StartCoroutine(BossState());
    }

    private void Update()
    {
        PriorityTarget();
        if (time > 0 && !anim.GetBool("isAttack"))
        {
            time -= Time.deltaTime;
            canAttack = false;
        }
        else if (time <= 0)
        {
            canAttack = true;
        }
        if (isBackward)
        { 
            BackWards();
        }
        if (anim.GetBool(hashDash))
        {
            DashAttackMove();
        }
        else
        {
            dashSpeed = 0;
            dashTime = 0;
        }
        if (anim.GetBool(hashJump))
        {
            if (canJump)
            {
                StartCoroutine(JumpAttackMove());   
            }
        }
        LookAt();
    }

    protected virtual void LookAt()
    {
        transform.LookAt(new Vector3(chaseTarget.position.x, transform.position.y, chaseTarget.position.z));
    }

    private IEnumerator BossState()
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

            if (wave == lastWave)
            {
                float distance = Vector3.Distance(chaseTarget.position, bossTr.position);
                if (distance <= attackRange)
                {
                    FreezeVelocity();
                    if (canAttack && !anim.GetBool("isAttack"))
                    {
                        CloseAttack();
                        foreach (SphereCollider coll in attackC)
                        { 
                            coll.enabled = true; 
                        }
                        jumpAttackC.enabled = false;
                        dashAttackC.enabled = false;
                    }
                    else
                    {
                        nav.enabled = true;
                        stateMachine.ChangeState(State.IDLE);
                    }
                }
                else if (distance <= amongRange && distance > attackRange)
                {
                    nav.enabled = true;
                    stateMachine.ChangeState(State.TRACE);
                    foreach (SphereCollider coll in attackC)
                    {
                        coll.enabled = false;
                    }
                    jumpAttackC.enabled = false;
                    dashAttackC.enabled = false;
                }
                else if (distance <= specialAttackRange && distance > amongRange)
                {
                    FreezeVelocity();
                    if (canAttack && !anim.GetBool("isAttack"))
                    {
                        StandoffAttack();
                    }
                    else
                    {
                        nav.enabled = true;
                        stateMachine.ChangeState(State.IDLE);
                    }
                }
                else if (distance > specialAttackRange && !anim.GetBool(hashJump))
                {
                    nav.enabled = true;
                    stateMachine.ChangeState(State.TRACE);
                    foreach (SphereCollider coll in attackC)
                    {
                        coll.enabled = false;
                    }
                    jumpAttackC.enabled = false;
                    dashAttackC.enabled = false;
                }
            }
            else
            {
                nav.enabled = true;
                stateMachine.ChangeState(State.IDLE);
                foreach (SphereCollider coll in attackC)
                {
                    coll.enabled = false;
                }
                jumpAttackC.enabled = false;
                dashAttackC.enabled = false;
            }
        }
    }

    private void CloseAttack()
    {
        int pattern = Random.Range(0, 2);
        switch (pattern)
        {
            case 0:
                stateMachine.ChangeState(State.DefaultA);
                break;
            case 1:
                stateMachine.ChangeState(State.DefaultA2);
                break;
        }
    }
    private void StandoffAttack()
    {
        int pattern = Random.Range(0, 4);
        switch (pattern)
        {
            case 0:
                stateMachine.ChangeState(State.JumpA);
                break;
            case 1:
                stateMachine.ChangeState(State.DashA);
                DashAttack_backward();
                break;
            default:
                stateMachine.ChangeState(State.TRACE);
                break;
        }
    }

    private void JumpAttack()
    {
        anim.SetBool(hashJump, true);
    }
    private IEnumerator JumpAttackMove()
    {
        canJump = false;
        nav.enabled = false;
        float distance = Vector3.Distance(chaseTarget.position, bossTr.position);
        Vector3 attackPos = transform.position + transform.forward * (distance - 2) + Vector3.up * 20.0f;
        mainCollider.isTrigger = true;
        yield return new WaitForSeconds(1.0f);
        foreach (SkinnedMeshRenderer render in renderer)
        {
            render.enabled = false;
        }
        FreezeVelocity();
        yield return new WaitForSeconds(0.1f);
        transform.position = attackPos;
        yield return new WaitForSeconds(1.0f);
        foreach (SkinnedMeshRenderer render in renderer)
        {
            render.enabled = true;
        }
        anim.SetBool(hashJump, false);
        jumpAttackC.enabled = true;
        yield return new WaitForSeconds(2.0f);
        mainCollider.isTrigger = false;
        nav.enabled = true;
        canJump = true;
    }

    private void DashAttack_backward()
    {
        isBackward = true;
    }
    private void BackWards()
    {
        transform.Translate(Vector3.back * 2f * Time.deltaTime);
    }

    private void DashAttack()
    {
        anim.SetBool(hashDash, true);
        isBackward = false;
    }

    private void DashAttackMove()
    {
        dashAttackC.enabled = true;      
        float speed = 0f;
        isDash = true;
        if (isDash == true)
        {
            dashSpeed += 0.001f;
            float distance = Vector3.Distance(chaseTarget.position, bossTr.position);
            Vector3 attackPos = transform.position + transform.forward * distance;
            transform.position = Vector3.MoveTowards(transform.position, attackPos, (speed + dashSpeed/2) + Time.deltaTime);
            dashTime += Time.deltaTime;
            if (dashTime > 2f)
            {
                Debug.Log("aa");
                anim.SetBool(hashDash, false);
                isDash = false;
                FreezeVelocity();
            }
        }
    }

    private void AttackDelay()
    {
        time = 1f;
    }

    private void AttackEnd()
    {
        anim.SetBool("isAttack", false);
    }

    private void PriorityTarget()                     //타겟 우선순위 설정
    {
        Collider[] turret = Physics.OverlapSphere(transform.position, sensingRange, turretLayer);
        if (turret.Length > 0)
        {
            turretDistance(turret);
            TargetingTurret();
            chaseTarget = turret[targetingIndex].transform;
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
        int secondMinIndex = 0;
        for (int i = 0; i < array.Length; i++)
        {
            distance[i] = Vector3.Distance(array[i].transform.position, bossTr.position);
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
            else if (distance[i] < secondDistance)
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
        Collider[] monster = Physics.OverlapBox(transform.position, new Vector3(5f, 5f, 5f), transform.rotation, monsterLayer);
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
        Gizmos.DrawWireCube(transform.position, new Vector3(5f, 5f, 5f));
    }

    protected void FreezeVelocity()                     //물리력 제거
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void UpScaleHp()                          //체력 증가량
    {
        hp += upScaleHp * wave;
    }

    public void Hurt(float damage)                   //플레이어에게 데미지 입을 시
    {
        hp -= damage;
    }

    public void isDie()                              //죽었을 시
    {
        isDead = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Turret") || other.gameObject.CompareTag("Monster"))
        {
            if (isDash)
            {
                isDash = false;
                anim.SetBool(hashDash, false);
                FreezeVelocity();
            }
        }
    }

    protected class BaseMonsterState : BaseState
    {
        protected BossMonster owner;
        public BaseMonsterState(BossMonster owner)
        { this.owner = owner; }
    }
    protected class IdleState : BaseMonsterState
    {
        public IdleState(BossMonster owner) : base(owner) { }
        public override void Enter()
        {
            owner.nav.isStopped = true;
            owner.anim.SetBool(owner.hashTrace, false);
        }
    }
    protected class TraceState : BaseMonsterState
    {
        public TraceState(BossMonster owner) : base(owner) { }
        public override void Enter()
        {
            owner.nav.SetDestination(owner.chaseTarget.position);
            owner.nav.isStopped = false;
            owner.anim.SetBool(owner.hashTrace, true);
        }
    }
    protected class JumpAState : BaseMonsterState
    {
        public JumpAState(BossMonster owner) : base(owner) { }
        public override void Enter()
        {
            owner.anim.SetBool(owner.hashAttack, true);
            owner.anim.SetTrigger(owner.hashJumpA);
        }
    }
    protected class DashAState : BaseMonsterState
    {
        public DashAState(BossMonster owner) : base(owner) { }
        public override void Enter()
        {
            owner.nav.isStopped = true;
            owner.anim.SetBool(owner.hashAttack, true);
            owner.anim.SetTrigger(owner.hashDashA);
        }
    }
    protected class DefaultAState : BaseMonsterState
    {
        public DefaultAState(BossMonster owner) : base(owner) { }
        public override void Enter()
        {
            owner.anim.SetBool(owner.hashAttack, true);
            owner.anim.SetTrigger(owner.hashDefaultA);
        }
    }    protected class DefaultA2State : BaseMonsterState
    {
        public DefaultA2State(BossMonster owner) : base(owner) { }
        public override void Enter()
        {
            owner.anim.SetBool(owner.hashAttack, true);
            owner.anim.SetTrigger(owner.hashDefaultA2);
        }
    }
    protected class DieState : BaseMonsterState
    {
        public DieState(BossMonster owner) : base(owner) { }
        public override void Enter()
        {
            owner.anim.SetTrigger(owner.hashDie);
        }
    }
}
