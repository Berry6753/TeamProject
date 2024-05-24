using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Core_Upgrade_UI_TextUpdate : MonoBehaviour
{
    [Header("UpScaleHP")]
    [SerializeField]
    private TMP_Text upscaleHP;

    [Header("useGear")]
    [SerializeField]
    private TMP_Text useGear;

    [Header("OwnGear")]
    [SerializeField]
    private TMP_Text ownGear;

    private Core core;
    private Player_Info player;

    private void Awake()
    {
        player = GameManager.Instance.GetPlayer.GetComponent<Player_Info>();
        core = GameManager.Instance.GetCore.GetComponent<Core>();
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            upscaleHP.text = $"{core.GetMaxHP} => {core.GetMaxHP + core.GetHPRise}";
            useGear.text = $"{core.upgradeCost}";
            ownGear.text = $"{player.GearCount}";
        }
    }
}
