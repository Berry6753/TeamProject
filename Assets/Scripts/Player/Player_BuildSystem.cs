using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_BuildSystem : MonoBehaviour
{
    private float SelectBuildTurretIndex;
    [SerializeField] private LayerMask mask;

    private List<string> poolDicTag = new List<string>();

    public float BuildModeOn {  get; private set; }

    GameObject build;

    private Vector3 buildPos;

    private void Awake()
    {
        BuildModeOn = -1f;
        SelectBuildTurretIndex = 0;
        buildPos = transform.position + new Vector3(5f,0,0);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        if(Physics.Raycast(ray, out RaycastHit hit, 8f, mask))
        {
            buildPos = hit.point;
        }

        return buildPos;        
    }

    public void OnSelectTurret(InputAction.CallbackContext context)
    {
        if(context.ReadValue<float>() > 0.5f)
        {
            SelectBuildTurretIndex++;

            if(SelectBuildTurretIndex >= MultiObjectPool.inst.poolDictionary.Count)
            {
                SelectBuildTurretIndex = 0;
            }         
        }
        else if(context.ReadValue<float>() < 0.5f)
        {
            SelectBuildTurretIndex--;

            if (SelectBuildTurretIndex < 0)
            {
                SelectBuildTurretIndex = MultiObjectPool.inst.poolDictionary.Count -1 ;
            }
        }

        if(build != null)
        {
            build.SetActive(false);
            build = MultiObjectPool.SpawnFromPool(poolDicTag[(int)SelectBuildTurretIndex], GetMouseWorldPosition());
        }
    }

    public void OnChangeBuildMode(InputAction.CallbackContext callback)
    {
        BuildModeOn *= -1;

        if(BuildModeOn > 0f)
        {
            foreach (var item in MultiObjectPool.inst.poolDictionary)
            {
                if (!poolDicTag.Contains(item.Key))
                {
                    poolDicTag.Add(item.Key);
                }
            }

            build = MultiObjectPool.SpawnFromPool(poolDicTag[(int)SelectBuildTurretIndex], GetMouseWorldPosition());
        }
        else
        {
            build.SetActive(false);
        }
    }

    private void Update()
    {
        CreateBuilding();
        //BuildTurret();
    }

    private void CreateBuilding()
    {
        if (BuildModeOn > 0f)
        {           
            if(build != null)
            {
                build.transform.position = GetMouseWorldPosition();
                build.transform.rotation = transform.rotation;
            }
        }
    }

    public void BuildTurret()
    {
        if (BuildModeOn < 0f) return;
        if (GetComponent<Player_Aiming>().isFire)
        {
            MultiObjectPool.SpawnFromPool(poolDicTag[(int)SelectBuildTurretIndex], build.transform.position, transform.rotation);
        }        
    }
}
