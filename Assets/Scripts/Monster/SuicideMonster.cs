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
        defaultTarget = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }
    private void Start()
    {
        chaseTarget = defaultTarget;
        ChaseTarget();
    }

    protected override void Update()
    {
        base.Update();
        LookAt();
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
                //FreezeVelocity();
                stateMachine.ChangeState(State.IDLE);
                yield return new WaitForSeconds(explosionTime);
                stateMachine.ChangeState(State.ATTACK);
                foreach (Collider c in attack)
                {
                    c.enabled = true;
                }
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

    protected override void UpScaleDamage()
    {
        if (wave % 5 == 0 && wave / 5 > 2)
        {
            damage *= 2;
        }
    }
}
