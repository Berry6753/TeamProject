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
    GameObject deleteBuild;

    private Vector3 buildPos;
    private Ray ray;

    private void Awake()
    {
        BuildModeOn = -1f;
        SelectBuildTurretIndex = 0;
        buildPos = transform.position + new Vector3(5f,0,0);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        ray = Camera.main.ScreenPointToRay(screenCenterPoint);

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

            //build = MultiObjectPool.SpawnFromPool(poolDicTag[(int)SelectBuildTurretIndex], GetMouseWorldPosition());
        }
        else
        {
            //build.SetActive(false);
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
            if (Physics.Raycast(Camera.main.transform.position, (GetMouseWorldPosition() - Camera.main.transform.position).normalized, out RaycastHit hit, 8f, mask))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.green);
                Debug.Log(hit.transform.name);

                if (hit.transform.CompareTag("Turret"))
                {
                    Debug.Log("설치된 터렛 찾음");
                    deleteBuild = hit.transform.gameObject;

                    if (build != null)
                    {
                        build.SetActive(false);
                        build = null;
                    }
                }
                else if (build != null)
                {
                    build.transform.position = GetMouseWorldPosition();
                    build.transform.rotation = transform.rotation;
                }
                else
                {
                    deleteBuild = null;
                    build = MultiObjectPool.SpawnFromPool(poolDicTag[(int)SelectBuildTurretIndex], GetMouseWorldPosition());
                }
            }
        }
        else
        {
            if (build != null) build.SetActive(false);
            build = null;
        }
    }

    public void BuildTurret()
    {
        if (BuildModeOn < 0f) return;
        if (GetComponent<Player_Aiming>().isFire)
        {
            if(deleteBuild != null)
            {
                deleteBuild.SetActive(false);
            }
            else
            {
                GameObject BuilingTurret = MultiObjectPool.SpawnFromPool(poolDicTag[(int)SelectBuildTurretIndex], build.transform.position, transform.rotation);
                var childs = BuilingTurret.GetComponentsInChildren<Collider>();
                foreach (var turretCollider in childs)
                {
                    turretCollider.gameObject.layer = LayerMask.NameToLayer("Turret");
                    turretCollider.tag = "Turret";
                }

                //BuilingTurret.layer = LayerMask.NameToLayer("Turret");
                //BuilingTurret.tag = "Turret";
            }
        }        
    }
}
