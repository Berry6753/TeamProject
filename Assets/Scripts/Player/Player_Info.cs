using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Info : MonoBehaviour
{
    //Level
    private int Level;

    [Header("최대 체력")]
    [SerializeField]
    private float maxHp;

    public float HP {  get; private set; }
    public int GearCount { get; private set; }

    [Header("공격력")]
    [SerializeField]
    private float ATKDamage;

    [Header("최대 탄 수")]
    [SerializeField]
    private float maxBullet;

    [Header("최대 탄창 수")]
    [SerializeField]
    private float maxMagazine;

    [Space(10)]
    [Header("기본 이동 속도")]
    [SerializeField]
    private float DefaultSpeed;
    public float defaultSpeed { get { return DefaultSpeed; } }

    [Space(10)]
    [Header("달리기 이동 속도")]
    [SerializeField]
    private float RunSpeed;
    public float runSpeed { get { return RunSpeed; } }

    public float Attack { get { return ATKDamage; } }

    public float maxEquipedBulletCount {  get; private set; }
    [HideInInspector]
    public float equipedBulletCount;

    public float maxMagazineCount {  get; private set; }
    [HideInInspector]
    public float magazineCount;

    private Animator animator;
    private bool isDead;

    private Player_Info_UI UI;

    private readonly int hashHurt = Animator.StringToHash("Hurt");
    private readonly int hashDead = Animator.StringToHash("Die");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        UI = GetComponent<Player_Info_UI>();        
    }

    private void OnEnable()
    {
        maxEquipedBulletCount = maxBullet;
        maxMagazineCount = maxMagazine;

        equipedBulletCount = maxEquipedBulletCount;
        magazineCount = maxMagazineCount;

        Level = 0;
        HP = maxHp;

        GearCount = 100;
        UI.InitGearText(GearCount);
    }

    public void AddGearCount(int gearCount)
    {
        GearCount += gearCount;
        UI.ChangeGearText(GearCount, gearCount);
    }

    public void UseGear(int gearCount)
    {
        GearCount -= gearCount;
        UI.ChangeGearText(GearCount, -gearCount);
    }

    public void Heal(float healValue)
    {
        HP = Mathf.Clamp(HP + healValue, 0, maxHp);
        UI.PrintPlayerHPBar(HP, maxHp);
    }

    public void Hurt(float damage)
    {
        HP -= damage;
        UI.PrintPlayerHPBar(HP, maxHp);
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
        animator.SetBool(hashDead, true);
    }

    //Dead Animation이 끝나면 실행되는 메서드
    public void GameOver()
    {
        //게임 오버 문구
        //게임 오버 메뉴 등장
    }

    public void OnDebug_Dead(InputAction.CallbackContext context)
    {
        if (context.started) return;
        if (context.performed)
        {
            Hurt(50);
        }
    }

    public void UpgradePlayer()
    {
        Debug.Log("업그레이드 완료");
        Level++;
        maxHp += 20;
        HP = maxHp;
        ATKDamage += 3;
        //이동 속도 증가
        DefaultSpeed += 1f;
        RunSpeed += 1f;
    }


}
