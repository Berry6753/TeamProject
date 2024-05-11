using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LookAtAnimationRigging : MonoBehaviour
{
    private List<MultiAimConstraint> rigs;
    private float targetWeight = 0f;

    private Animator animator;

    private readonly int hashAiming = Animator.StringToHash("Aiming");

    private void Awake()
    {
        animator = GameObject.FindWithTag("Player").GetComponent<Animator>();
        rigs = new List<MultiAimConstraint>();

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            rigs.Add(transform.GetChild(i).GetComponent<MultiAimConstraint>());
            rigs[i].weight = 1f;
        }
    }

    private void Update()
    {
        ChangeWeight();
        //rigs[1].weight = Mathf.Lerp(rigs[1].weight, targetWeight, Time.deltaTime * 10f);
        rigs[3].weight = Mathf.Lerp(rigs[3].weight, targetWeight, Time.deltaTime * 10f);
        rigs[4].weight = Mathf.Lerp(rigs[4].weight, targetWeight, Time.deltaTime * 10f);
    }

    private void ChangeWeight()
    {
        if (animator.GetBool(hashAiming))
        {
            targetWeight = 1f;
        }
        else
        {
            targetWeight = 0f;
        }
    }
}
