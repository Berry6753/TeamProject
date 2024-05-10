using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LookAtAnimationRigging : MonoBehaviour
{
    //private List<MultiAimConstraint> rigs;
    private Rig rig;
    private float targetWeight = 1f;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rig = GetComponent<Rig>();
        //rigs = new List<MultiAimConstraint>();

        //for (int i = 0; i < transform.childCount-1; i++)
        //{
        //    rigs.Add(transform.GetChild(i).GetComponent<MultiAimConstraint>());
        //    rigs[i].weight = 1f;
        //}
    }

    private void Update()
    {
        ChangeWeight();
        rig.weight = Mathf.Lerp(rig.weight, targetWeight, Time.deltaTime * 10f);        
        //ChangeWeight();
    }

    //private void ChangeWeight()
    //{
    //    if (animator.GetBool("Aiming"))
    //    {
    //        rigs[0].weight = 0;
    //        rigs[2].weight = 0;
    //    }
    //    else
    //    {
    //        rigs[0].weight = 1;
    //        rigs[2].weight = 1;
    //    }
    //}

    private void ChangeWeight()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            targetWeight = 1f;
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            targetWeight = 0f;
        }
    }
}
