using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Command_UI_SelectMenu : MonoBehaviour
{
    private GameObject player;

    private Player_Info Player_Info;
    private Core core;

    private void Awake()
    {
        player = GameManager.Instance.GetPlayer;
        core = GameManager.Instance.GetCore.GetComponent<Core>();

        Player_Info = player.GetComponent<Player_Info>();
        
    }

    public void UpgradePlayerSelect()
    {
        if (!Player_Info.UpgradePlayer())
        {
            //실패 문구 On

            Debug.Log("Upgrade 실패");
        }
    }

    public void UpgradeCoreSelect()
    {
        if(Player_Info.GearCount < core.upgradeCost)
            core.isUpgrading = true;
    }

    public void ReloadCoreSelect()
    {
        core.Reloading();
    }


}
