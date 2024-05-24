using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Command_UI_SelectMenu : MonoBehaviour
{

    [Header("Gear UPgrade Fail Text")]
    [SerializeField]
    private TMP_Text GearFailText;

    [Header("Player UPgrade Fail Text")]
    [SerializeField]
    private TMP_Text PlayerFailText;

    [Header("Reload UPgrade Fail Text")]
    [SerializeField]
    private TMP_Text ReloadFailText;

    private GameObject player;
    private Player_Info player_Info;
    private Player_Command player_Command;
    private Core core;
    private Animator coreAnime;

    private LayerMask defualtMask;

    private readonly int hashCommand = Animator.StringToHash("Zoom");

    private void Awake()
    {
        player = GameManager.Instance.GetPlayer;
        core = GameManager.Instance.GetCore.GetComponent<Core>();
        coreAnime = core.GetComponent<Animator>();

        player_Info = player.GetComponent<Player_Info>();
        player_Command = player.GetComponent<Player_Command>();

        defualtMask = player.GetComponent<Player_Command>().defualtMask;
    }

    public void UpgradePlayerSelect()
    {
        PlayerFailText.enabled = true;
        if (!player_Info.UpgradePlayer())
        {
            PlayerFailText.color = Color.red;
            PlayerFailText.text = "Not Enough Gear";
        }
        else
        {
            PlayerFailText.color = Color.yellow;
            PlayerFailText.text = "Upgrade Complete.";
        }
    }

    public void UpgradeCoreSelect()
    {
        GearFailText.enabled = true;
        if (player_Info.GearCount >= core.upgradeCost)
        {
            if (!core.isUpgrading)
            {
                GearFailText.color = Color.yellow;
                GearFailText.text = "Upgrade Complete.";

                core.isUpgrading = true;
                player_Info.UseGear(core.upgradeCost);
            }
            else
            {
                GearFailText.color = Color.red;
                GearFailText.text = "Currently being upgraded.";
            }

        }
        else
        {
            GearFailText.color = Color.red;
            GearFailText.text = "Not Enough Gear";
        }
    }

    public void FailTextClear()
    {
        GearFailText.enabled = false;
        PlayerFailText.enabled = false;
        ReloadFailText.enabled = false;
    }

    public void ReloadCoreSelect()
    {
        ReloadFailText.enabled = true;
        if (core.isReloading)
        {
            ReloadFailText.color = Color.yellow;
            ReloadFailText.text = "Reload Complete";
            core.Reloading();
        }
        else
        {
            ReloadFailText.color = Color.red;
            ReloadFailText.text = "Is CoolTIme";
        }
        
    }

    public void CloseCommandUI()
    {
        GameManager.Instance.isGameStop = -1;
        GameManager.Instance.GetCoreUI.SetActive(false);
        
        Camera.main.cullingMask = defualtMask;
        coreAnime.SetBool(hashCommand, false);
    }
}
