using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMonster : MonoBehaviour
{
    [Header("스탯")]
    [SerializeField] protected float hp;                //체력
    [SerializeField] protected float damage;            //공격력
    [SerializeField] protected float hitNum;            //타격 횟수
    [SerializeField] protected float attackRange;       //사거리
    [SerializeField] protected float specialAttackRange;//특수 공격 사거리
    [Header("스탯 성장치")]
    [SerializeField] protected float upScaleHp;         //체력 성장치

    private Transform playerTr;
    private Transform bossTr;

    private Animator anim;
    private NavMeshAgent nav;

    private StateMachine stateMachine;

    private readonly int hashTrace = Animator.StringToHash("isTrace");
    private readonly int hashAttack = Animator.StringToHash("isAttack");
    private readonly int hashGetHit = Animator.StringToHash("isGetHit");
    private readonly int hashDie = Animator.StringToHash("isDie");

    public enum State
    { IDLE, TRACE, ATTACK, DIE, GETHIT, SPAWN }
    public State state = State.IDLE;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        bossTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        stateMachine = gameObject.AddComponent<StateMachine>();

        stateMachine.AddState(State.IDLE, new IdleState(this));
        stateMachine.AddState(State.TRACE, new TraceState(this));
        stateMachine.AddState(State.ATTACK, new AttackState(this));
        stateMachine.AddState(State.GETHIT, new GetHitState(this));
        stateMachine.AddState(State.DIE, new DieState(this));
        stateMachine.InitState(State.IDLE);
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
            owner.nav.SetDestination(owner.playerTr.position);
            owner.nav.isStopped = false;
            owner.anim.SetBool(owner.hashTrace, true);
            owner.anim.SetBool(owner.hashAttack, false);
        }
    }
    protected class AttackState : BaseMonsterState
    {
        public AttackState(BossMonster owner) : base(owner) { }
        public override void Enter()
        {
            owner.anim.SetBool(owner.hashAttack, true);
        }
    }
    protected class GetHitState : BaseMonsterState
    {
        public GetHitState(BossMonster owner) : base(owner) { }
        public override void Enter()
        {
            owner.anim.SetTrigger(owner.hashGetHit);
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
