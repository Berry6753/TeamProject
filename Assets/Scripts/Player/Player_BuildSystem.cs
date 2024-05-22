using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_BuildSystem : MonoBehaviour
{
    [SerializeField] private int SelectBuildTurretIndex;
    [SerializeField] private LayerMask mask;

    private Player_Aiming aiming;
    private Player_Info info;

    private List<string> poolDicTag = new List<string>();

    public float BuildModeOn {  get; private set; }

    GameObject build;
    GameObject deleteBuild;
    public GameObject selectBuild { get { return deleteBuild; } }

    private Vector3 buildPos;
    private Ray ray;

    private Vector2 screenCenterPoint;

    private bool isUpgradeAble;
    private bool isRepairAble;
    public bool isUpgrade { get { return isUpgradeAble; } }
    public bool isRepair { get { return isRepairAble; } }

    private void Awake()
    {
        aiming = GetComponent<Player_Aiming>();
        info = GetComponent<Player_Info>();

        BuildModeOn = -1f;
        SelectBuildTurretIndex = 0;
    }

    private void Start()
    {
        TurretIcon_UI.instance.PrintSelectTurret(SelectBuildTurretIndex);
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
        if (aiming.isGameStop > 0) return;
        if (context.performed)
        {
            if (context.ReadValue<float>() > 0.5f)
            {
                SelectBuildTurretIndex++;

                if (SelectBuildTurretIndex >= MultiObjectPool.inst.poolDictionary.Count)
                {
                    SelectBuildTurretIndex = 0;
                }
            }
            else if (context.ReadValue<float>() < 0.5f)
            {
                SelectBuildTurretIndex--;

                if (SelectBuildTurretIndex < 0)
                {
                    SelectBuildTurretIndex = MultiObjectPool.inst.poolDictionary.Count - 1;
                }
            }

            TurretIcon_UI.instance.PrintSelectTurret(SelectBuildTurretIndex);

            if (build != null)
            {
                build.SetActive(false);
                build = MultiObjectPool.SpawnFromPool(poolDicTag[SelectBuildTurretIndex], GetMouseWorldPosition());
            }
        }
        
    }

    public void OnChangeBuildMode(InputAction.CallbackContext callback)
    {
        if (aiming.isGameStop > 0) return;

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
        }
    }
    public void OnUpgradeTurret(InputAction.CallbackContext context)
    {
        if (context.started) return;
        if (context.performed)
        {
            //if (BuildModeOn < 0f) return;
            //if (deleteBuild == null) return;
            if (BuildModeOn < 0f) return;
            if (!isUpgradeAble) return;

            deleteBuild.GetComponent<Turret>().TurretUpgrade();
            info.UseGear((int)deleteBuild.GetComponent<Turret>().turretUpgradCost);
        }
    }

    public void OnRepairTurret(InputAction.CallbackContext context)
    {
        if (context.started) return;
        if (context.performed)
        {
            if (aiming.isGameStop > 0) return;
            if (BuildModeOn < 0f) return;
            if (!isRepairAble) return;

            deleteBuild.GetComponent<Turret>().TurretRepair();
            info.UseGear((int)deleteBuild.GetComponent<Turret>().turretRepairCost);
        }
        
        
    }

    private void Update()
    {
        CreateBuilding();
        PrintTurretUpgradeAble();
        PrintTurretRepairAble();
        //UpgradeTurret();
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
                if (Mathf.Abs(Vector3.Dot(hit.normal, Vector3.up)) <= 0.5f) 
                {
                    DeleteBuild();
                    //return;
                }

                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Turret"))
                {
                    Debug.Log("설치된 터렛 찾음");
                    deleteBuild = hit.transform.gameObject;
                    Debug.Log(deleteBuild.name);
                    
                    DeleteBuild();
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
                    build = MultiObjectPool.SpawnFromPool(poolDicTag[SelectBuildTurretIndex], GetMouseWorldPosition());
                }
            }
            else
            {
                deleteBuild = null;
                DeleteBuild();
            }        
        }
        else
        {
            deleteBuild = null;
            DeleteBuild();
        }
    }

    private void DeleteBuild()
    {
        if (build != null)
        {
            build.SetActive(false);
            build = null;
        }
    }

    private void PrintTurretUpgradeAble()
    {
        if (deleteBuild != null)
        {
            Turret upgradeTurret = deleteBuild.GetComponent<Turret>();
            if (upgradeTurret == null) return;
            if (upgradeTurret != null && info.GearCount >= upgradeTurret.turretUpgradCost)
            {
                isUpgradeAble = true;
            }
            else isUpgradeAble = false;
        }
        else isUpgradeAble = false;


        //Debug.Log($"업그레이드 가능 여부 : {isUpgradeAble}");
    }

    private void PrintTurretRepairAble()
    {
        if (deleteBuild != null)
        {
            Turret repairTurret = deleteBuild.GetComponent<Turret>();
            if (repairTurret == null) return;
            if (repairTurret.isTurretRepair && info.GearCount >= repairTurret.turretRepairCost)
            {
                isRepairAble = true;
            }
            else isRepairAble=false;
        }
        else isRepairAble= false;
    }

    public void BuildTurret()
    {
        if (BuildModeOn < 0f) return;

        if (GetComponent<Player_Aiming>().isFire)
        {          
            if(deleteBuild != null)
            {
                float destroyTurretGearCount = 0f;

                if(deleteBuild.GetComponent<Turret>() != null)
                {
                    destroyTurretGearCount = deleteBuild.GetComponent<Turret>().turretMakingCost;
                }
                else if (deleteBuild.GetComponent<Barrel>() != null)
                {
                    destroyTurretGearCount = deleteBuild.GetComponent<Barrel>().makingCost;
                }
                

                //선택된 터렛의 상태를 destory로 변경

                //디버그 전용
                deleteBuild.transform.parent.gameObject.SetActive(false);
                deleteBuild = null;

                //해당 포탑의 생성 기어의 50% 회수
                info.AddGearCount((int)Mathf.Round(destroyTurretGearCount * 0.5f));
            }
            else
            {
                if (build == null) return;
                if(build.transform.GetChild(1).GetComponent<Turret>() != null)
                {
                    if (build.transform.GetChild(1).GetComponent<Turret>().isMake)
                    {
                        CreateTurretPrecondition();
                    }
                }
                else if(build.transform.GetChild(0).GetComponent<Barrel>() != null)
                {
                    if (build.transform.GetChild(0).GetComponent<Barrel>().isMake)
                    {
                        CreateTurretPrecondition();
                    }
                }
               
            }
        }        
    }

    private void CreateTurretPrecondition()
    {
        int buildTurretGearCount;

        if (build.transform.GetChild(1).GetComponent<Turret>() != null)
        {
            buildTurretGearCount = (int)build.transform.GetChild(1).GetComponent<Turret>().turretMakingCost;
        }
        else
        {
            buildTurretGearCount = build.transform.GetChild(0).GetComponent<Barrel>().makingCost;
        }

        if (info.GearCount < buildTurretGearCount)
        {
            Debug.Log("기어의 수가 부족합니다.");
            return;
        }

        //기어 소모
        info.UseGear(buildTurretGearCount);
        //터렛 생성
        GameObject BuilingTurret = MultiObjectPool.SpawnFromPool(poolDicTag[SelectBuildTurretIndex], build.transform.position, transform.rotation);

        //터렛의 상태를 Build or Making이라고 변경
        if (BuilingTurret.transform.GetChild(1).GetComponent<Turret>() != null)
        {
            BuilingTurret.transform.GetChild(1).GetComponent<Turret>().TurretMake();
        }
        else
        {
            BuilingTurret.transform.GetChild(0).GetComponent<Barrel>().Making();
        }

        //디버그 전용
        //생성된 터렛 태그와 레이어 변경
        //var childs = BuilingTurret.GetComponentsInChildren<Collider>();
        //foreach (var turretCollider in childs)
        //{
        //    turretCollider.gameObject.layer = LayerMask.NameToLayer("Turret");
        //    turretCollider.tag = "Turret";
        //    turretCollider.isTrigger = false;
        //}

        //BuilingTurret.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
}
