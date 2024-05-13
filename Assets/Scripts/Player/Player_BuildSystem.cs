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

    private Vector2 screenCenterPoint;

    private void Awake()
    {
        BuildModeOn = -1f;
        SelectBuildTurretIndex = 0;
    }

    private Vector3 GetMouseWorldPosition()
    {
        screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit hit, 8f, mask))
        {
            buildPos = hit.point;
        }
        else
        {
            buildPos = Camera.main.transform.position + new Vector3(transform.forward.x, 0, transform.forward.z) * 11.5f;
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
            screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
            ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            if (Physics.Raycast(ray, out RaycastHit hit, 8f, mask))
            {
                Debug.DrawLine(Camera.main.transform.position, hit.point, Color.green);
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
                    deleteBuild = null;
                    build.transform.position = GetMouseWorldPosition();
                    build.transform.rotation = transform.rotation;
                }
                else
                {
                    deleteBuild = null;
                    build = MultiObjectPool.SpawnFromPool(poolDicTag[(int)SelectBuildTurretIndex], GetMouseWorldPosition());
                }
            }
            else
            {
                if (build != null)
                {
                    build.transform.position = new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z) + new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * 8f;
                    build.transform.rotation = transform.rotation;
                }
                else
                {
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
                //선택된 터렛의 상태를 destory로 변경

                //디버그 전용
                deleteBuild.SetActive(false);
                deleteBuild = null;
            }
            else
            {
                if (build.GetComponent<_Test1>().isBuildAble)
                {
                    GameObject BuilingTurret = MultiObjectPool.SpawnFromPool(poolDicTag[(int)SelectBuildTurretIndex], build.transform.position, transform.rotation);

                    //터렛의 상태를 Build or Making이라고 변경

                    //삭제 예정
                    //생성된 터렛 태그와 레이어 변경
                    var childs = BuilingTurret.GetComponentsInChildren<Collider>();
                    foreach (var turretCollider in childs)
                    {
                        turretCollider.gameObject.layer = LayerMask.NameToLayer("Turret");
                        turretCollider.tag = "Turret";
                        turretCollider.isTrigger = false;
                    }

                    BuilingTurret.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                }

                //BuilingTurret.layer = LayerMask.NameToLayer("Turret");
                //BuilingTurret.tag = "Turret";
            }
        }        
    }
}
