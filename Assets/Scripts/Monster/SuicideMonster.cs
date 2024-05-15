using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SuicideMonster : Monster
{
    private float explosionTime = 3f;
    private float duringExplosion = 3f;
    [SerializeField] private ParticleSystem exlosionEffect;
    [SerializeField] private GameObject form;

    protected override void Awake()
    {
        base.Awake();
        defaltTarget = GameObject.FindWithTag("Player").GetComponent<Transform>();
        attack = GetComponentInChildren<SphereCollider>();
        attack.enabled = false;
    }
    private void Start()
    {
        chaseTarget = defaltTarget;
        ChaseTarget();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void ChaseTarget()
    {
        StartCoroutine(MonsterState());
    }

    protected override IEnumerator MonsterState()
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
                stateMachine.ChangeState(State.IDLE);
                yield return new WaitForSeconds(explosionTime);
                stateMachine.ChangeState(State.ATTACK);
                attack.enabled = true;
                exlosionEffect.Play();
                form.SetActive(false);
                yield return new WaitForSeconds(duringExplosion);
                Destroy(gameObject);
                yield break;
            }
            else
            {
                stateMachine.ChangeState(State.TRACE);
            }
        }
    }

    protected override void SpawnTiming()
    {
        throw new NotImplementedException();
    }

    protected override void UpScaleDamage()
    {
        if (wave % 5 == 0 && wave / 5 > 2)
        {
            damage *= 2;
        }
    }
}
