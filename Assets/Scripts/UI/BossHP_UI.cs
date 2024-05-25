using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BossHP_UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI boss_HP_Text;
    [SerializeField] private Slider boss_HP_Slider;

    private void Update()
    {
        boss_HP_Text.text = $"{GameManager.Instance.Boss.Hp}  /  {GameManager.Instance.Boss.MaxHp}";
        boss_HP_Slider.value = GameManager.Instance.Boss.Hp / GameManager.Instance.Boss.MaxHp;
    }
}
