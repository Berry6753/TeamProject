using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LookAtAnimationRigging : MonoBehaviour
{ 

    private List<MultiAimConstraint> rigs;
    private TwoBoneIKConstraint leftHandRig;

    private float aimingTargetWeight = 0f;
    private float reloadTargetWeight = 0f;
    private float gameStopTargetWeight = 0f;
    private float ThrowingTargetWeight = 0f;

    private Animator animator;

    private readonly int hashAiming = Animator.StringToHash("Aiming");
    private readonly int hashReload = Animator.StringToHash("Reload");
    private readonly int hashDead = Animator.StringToHash("Die");
    private readonly int hashThrow = Animator.StringToHash("Throw");

    private void Awake()
    {
        animator = GameObject.FindWithTag("Player").GetComponent<Animator>();
        rigs = new List<MultiAimConstraint>();        

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            rigs.Add(transform.GetChild(i).GetComponent<MultiAimConstraint>());
            rigs[i].weight = 1f;
        }

        leftHandRig = transform.GetChild(transform.childCount - 1).GetComponent<TwoBoneIKConstraint>();
    }

    private void Update()
    {
        ChangeWeight();
        ReloadWeight();
        GameStop();
        Throwing();
        Rigging();
        //rigs[1].weight = Mathf.Lerp(rigs[1].weight, targetWeight, Time.deltaTime * 10f);
        //rigs[3].weight = Mathf.Lerp(rigs[3].weight, targetWeight, Time.deltaTime * 10f);
        //rigs[4].weight = Mathf.Lerp(rigs[4].weight, targetWeight, Time.deltaTime * 10f);
    }

    private void ChangeWeight()
    {
        if (animator.GetBool(hashAiming) && !animator.GetBool(hashDead))
        {
            aimingTargetWeight = 1f;
        }
        else
        {
            aimingTargetWeight = 0f;
        }        
    }

    private void ReloadWeight()
    {
        if (animator.GetBool(hashReload))
        {
            reloadTargetWeight = 0f;
        }
        else
        {
            reloadTargetWeight = 1f;
        }        
    }

    private void Rigging()
    {
        rigs[0].weight = Mathf.Lerp(rigs[0].weight, reloadTargetWeight, Time.deltaTime * 10f);
        rigs[1].weight = Mathf.Lerp(rigs[1].weight, reloadTargetWeight * gameStopTargetWeight, Time.deltaTime * 10f);
        rigs[2].weight = Mathf.Lerp(rigs[2].weight, reloadTargetWeight * gameStopTargetWeight, Time.deltaTime * 10f);
        rigs[3].weight = Mathf.Lerp(rigs[3].weight, aimingTargetWeight * reloadTargetWeight, Time.deltaTime * 10f);
        rigs[4].weight = Mathf.Lerp(rigs[4].weight, aimingTargetWeight * reloadTargetWeight, Time.deltaTime * 10f);
        leftHandRig.weight = Mathf.Lerp(leftHandRig.weight, ThrowingTargetWeight * reloadTargetWeight, Time.deltaTime * 10f);
    }

    private void GameStop()
    {
        if(Time.timeScale == 0)
        {
            gameStopTargetWeight = 0f;
        }
        else
        {
            gameStopTargetWeight = 1f;
        }
    }

    private void Throwing()
    {
        if (animator.GetBool(hashThrow))
        {
            ThrowingTargetWeight = 0;
        }
        else
        {
            ThrowingTargetWeight = 1f;
        }
    }

}
