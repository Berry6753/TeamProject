using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class Player_Info : MonoBehaviour
{
    public float HP {  get; private set; }
    public float GearCount { get; private set; }
    public float Attack {  get; private set; }

    private Animator animator;
    private bool isDead;

    private readonly int hashHurt = Animator.StringToHash("Hurt");
    private readonly int hashDead = Animator.StringToHash("Dead");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Hurt(float damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            Dead();
        }
        else
        {
            animator.SetTrigger(hashHurt);
        }
    }

    private void Dead()
    {
        isDead = true;
        animator.SetTrigger(hashDead);
    }

    //Dead Animation이 끝나면 실행되는 메서드
    public void GameOver()
    {
        //게임 오버 문구
        //게임 오버 메뉴 등장
    }
}
