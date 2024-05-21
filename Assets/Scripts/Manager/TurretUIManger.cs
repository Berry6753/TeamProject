using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TurretUIManger : MonoBehaviour
{
    [SerializeField]
    Canvas canvas;
    Canvas canvesPrefab;
    Player_BuildSystem player;

    private void Awake()
    {
        
        canvesPrefab= Instantiate(canvas);
        Debug.Log(canvesPrefab.name);
        canvesPrefab.enabled = false;
        
        player = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Player_BuildSystem>();
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        if (player.selectBuild != null && player.selectBuild.CompareTag("Turret"))
        {
            if (player.selectBuild.GetComponent<Turret>().turretStateName != TurretStateName.MAKING &&
                player.selectBuild.GetComponent<Turret>().turretStateName != TurretStateName.UPGRADE &&
                player.selectBuild.GetComponent<Turret>().turretStateName != TurretStateName.REPAIR) 
            {
                canvesPrefab.enabled = true;
                canvesPrefab.transform.forward = Camera.main.transform.forward;
                canvesPrefab.transform.position = player.selectBuild.transform.position + new Vector3(0, 1, 0) + ((Camera.main.transform.position - player.selectBuild.transform.position) * 0.3f);
                if (!player.isUpgrade || !player.selectBuild.GetComponent<Turret>().isTurretUpgrade)
                {
                    canvesPrefab.transform.GetChild(0).GetComponent<CanvasRenderer>().SetAlpha(0.5f);
                }
                else
                {
                    canvesPrefab.transform.GetChild(0).GetComponent<CanvasRenderer>().SetAlpha(1);
                }

                if (!player.isRepair || !player.selectBuild.GetComponent<Turret>().isTurretRepair)
                {
                    canvesPrefab.transform.GetChild(1).GetComponent<CanvasRenderer>().SetAlpha(0.5f);
                }
                else
                {
                    canvesPrefab.transform.GetChild(1).GetComponent<CanvasRenderer>().SetAlpha(1);
                }

            }
            else
            {
                canvesPrefab.enabled = false;
            }

        }
        else
        {
            canvesPrefab.enabled = false;
        }

    }


}
