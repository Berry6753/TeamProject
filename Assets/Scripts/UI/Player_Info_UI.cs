using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player_Info_UI : MonoBehaviour
{
    [Header("체력 UI")]
    [SerializeField]
    private Image HP_Bar;

    [Header("Bullet UI")]
    [SerializeField]
    private TMP_Text bulletText;

    [Header("탄창 UI")]
    [SerializeField]
    private TMP_Text magazineText;

    [Header("플레이어의 기어 UI")]
    [SerializeField]
    private TMP_Text gearCount;

    [Header("Turret Icon UI")]
    [SerializeField]
    private Image turretIcon;

    [Header("Use Gear Text")]
    [SerializeField]
    private TMP_Text useGearCount;

    private Animator UseGearTextAnimator;

    [Header("Core UI")]
    [SerializeField]
    private RawImage coreView;
    [SerializeField]
    private Image coreViewBG;
    private float coreDistance = 20.0f;
    private GameObject core;

    private readonly int hashTrigger = Animator.StringToHash("Get");

    private void Awake()
    {
        UseGearTextAnimator = useGearCount.transform.parent.GetComponent<Animator>();
        core = GameManager.Instance.GetCore;
        coreView.enabled = false;
        coreViewBG.enabled = false;
    }

    private void Update()
    {
        if (!core.activeSelf)
        {
            coreView.enabled = false;
            coreViewBG.enabled = false;
            return;
        }

        if (coreDistance <= Vector3.Distance(transform.position, core.transform.position))
        {
            coreViewBG.enabled = true;
            coreView.enabled = true;
        }
        else
        {
            coreViewBG.enabled = false;
            coreView.enabled = false;
        }
        
    }

    public void PrintPlayerHPBar(float hp, float maxHP)
    {
        HP_Bar.fillAmount = hp/maxHP;
    }

    public void InitGearText(float GearCount)
    {
        gearCount.text = $"{GearCount}";
    }

    public void ChangeGearText(float GearCount, float changeGearCount)
    {
        gearCount.text = $"{GearCount}";
        if(changeGearCount > 0)
        {
            useGearCount.text = $"+{changeGearCount}";
        }
        else
        {
            useGearCount.text = $"{changeGearCount}";
        }


        UseGearTextAnimator.SetTrigger(hashTrigger);
    }

    public void ChangeFireText(float equipedBulletCount)
    {
        bulletText.text = $"{equipedBulletCount} / 30";
        //Debug.Log($"{bulletText.text}");
    }

    public void Reload(float equipedBulletCount, float magazineCount)
    {
        bulletText.text = $"{equipedBulletCount} / 30";
        magazineText.text = $"{magazineCount} / 8";
    }
}
