using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Player_Info_UI : MonoBehaviour
{
    private Player_Info Player_Info;

    [Header("Ã¼·Â UI")]
    [SerializeField]
    private Image HP_Bar;

    [Header("Bullet UI")]
    [SerializeField]
    private TMP_Text bulletText;

    [Header("ÅºÃ¢ UI")]
    [SerializeField]
    private TMP_Text magazineText;

    private void Awake()
    {
         Player_Info = GetComponent<Player_Info>();
    }

    public void Fire()
    {
        bulletText.text = $"{Player_Info.equipedBulletCount} / 30";
    }

    public void Reload()
    {
        bulletText.text = $"{Player_Info.equipedBulletCount} / 30";
        magazineText.text = $"{Player_Info.magazineCount} / 8";
    }
}
