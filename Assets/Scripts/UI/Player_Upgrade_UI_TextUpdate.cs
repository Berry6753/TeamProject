using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player_Upgrade_UI_TextUpdate : MonoBehaviour
{
    [Header("UpScaleHP")]
    [SerializeField]
    private TMP_Text upscaleHP;

    [Header("UpScaleDM")]
    [SerializeField]
    private TMP_Text upscaleDM;

    [Header("useGear")]
    [SerializeField]
    private TMP_Text useGear;

    [Header("OwnGear")]
    [SerializeField]
    private TMP_Text ownGear;

    private Player_Info player;

    private void Awake()
    {
        player = GameManager.Instance.GetPlayer.GetComponent<Player_Info>();
    }

    private void Update()
    {
        if(gameObject.activeSelf)
        {
            upscaleHP.text = $"{player.GetmaxHP} => {player.GetmaxHP + 20}";
            upscaleDM.text = $"{player.GetATKDamage} => {player.GetATKDamage * 1.2f}";
            useGear.text = $"{player.GetLevelCost}";
            ownGear.text = $"{player.GearCount}";
        }
    }
}
