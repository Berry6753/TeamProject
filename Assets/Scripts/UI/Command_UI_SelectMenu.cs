using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Command_UI_SelectMenu : MonoBehaviour
{
    private GameObject player;

    private Player_Info player_Info;
    private Player_Command player_Command;
    private Core core;

    private void Awake()
    {
        player = GameManager.Instance.GetPlayer;
        core = GameManager.Instance.GetCore.GetComponent<Core>();

        player_Info = player.GetComponent<Player_Info>();
        player_Command = player.GetComponent<Player_Command>();
    }

    public void UpgradePlayerSelect()
    {
        if (!player_Info.UpgradePlayer())
        {
            //실패 문구 On

            Debug.Log("Upgrade 실패");
        }
    }

    public void UpgradeCoreSelect()
    {
        if(player_Info.GearCount < core.upgradeCost)
            core.isUpgrading = true;
    }

    public void ReloadCoreSelect()
    {
        core.Reloading();
    }

    public void EndCommandMode()
    {
        GameManager.Instance.isGameStop = -1;
        player_Command.isCommand = false;
        GameManager.Instance.GetPlayerUI.SetActive(true);
    }
}
