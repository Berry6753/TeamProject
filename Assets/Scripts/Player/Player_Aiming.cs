using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.Windows;

public class Player_Aiming : MonoBehaviour
{
    private Animator animator;
    public bool isAiming;

    public bool isFire {  get; private set; }

    private readonly int hashAiming = Animator.StringToHash("Aiming");
    private readonly int hashZoomOn = Animator.StringToHash("ZoomOn");
    private readonly int hashFire = Animator.StringToHash("Fire");
    private readonly int hashReload = Animator.StringToHash("Reload");

    [Header("Aiming ī�޶�")]
    [SerializeField]
    private GameObject Camera2;

    [Header("ī�޶� LookAt")]
    [SerializeField]
    private Transform CameraLookAt;

    [Header("��")]
    [SerializeField]
    private Transform gun;

    [Header("�ѱ�")]
    [SerializeField]
    private Transform GunFireStartPoint;

    [Header("������")]
    [SerializeField]
    private Transform aim;

    public Cinemachine.AxisState x_Axis;
    public Cinemachine.AxisState y_Axis;

    [Header("���� LayerMask")]
    [SerializeField]
    private LayerMask aimColliderLayerMask = new LayerMask();

    [Header("�ѱ� LayerMask")]
    [SerializeField]
    private LayerMask FireColliderLayerMask = new LayerMask();

    [Header("Rig�� Target")]
    [SerializeField]
    private Transform debugTransform;

    private float AttackTimer;
    private bool AttackAble;

    [Header("���� ������")]
    [SerializeField]
    private float AttackDelayTime;

    private float notAimingTimer;
    private float notAimingDelayTime = 1.5f;

    [Header("�ѱ� ź�� ����")]
    [SerializeField]
    private float attackAccuracy;

    [Header("�ѱ� ī�޶� �ݵ�")]
    [SerializeField]
    private float cameraLerftime;

    [Header("�Ϲ� �ݵ�")]
    [SerializeField]
    float recoilback;

    [Header("���� �ݵ�")]
    [SerializeField]
    float aimingrecoilback;

    //�ѱ� �ݵ�
    private float recoilBackForce;

    [Header("���� ��ƼŬ")]
    [SerializeField]
    private Transform ParticleSystem;

    private Player_BuildSystem buildSystem;

    private Player_Info info;
    private Player_Info_UI UI;

    private Quaternion mouseRotation;

    [Header("�� ����� Ŭ��")]
    [SerializeField]
    private List<AudioClip> shootAudioClip;

    [Header("�� �ǰ� Effect")]
    [SerializeField]
    private GameObject effect;

    private AudioSource audioSource;

    private float stopGame;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        AttackTimer = 0;
        cameraLerftime = 1f;
        notAimingTimer = notAimingDelayTime;
        info = GetComponent<Player_Info>();

        buildSystem = GetComponent<Player_BuildSystem>();
        UI = GetComponent<Player_Info_UI>();
        audioSource = GetComponent<AudioSource>();
        stopGame = GameManager.Instance.isGameStop;
    }

    private Quaternion initRotation;

    private void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        InitRotation();
    }

    private void InitRotation()
    {
        x_Axis.Value = 0;
        y_Axis.Value = 0;

        initRotation = CameraLookAt.rotation;

        Vector3 initEulerAngle = initRotation.eulerAngles;
        x_Axis.Value = initEulerAngle.y;
        y_Axis.Value = initEulerAngle.x;

        mouseRotation = initRotation;
    }

    public void OnAiming(InputAction.CallbackContext context)
    {
        if (stopGame > 0) return;
        if (buildSystem.BuildModeOn > 0f) return;
        isAiming = context.ReadValue<float>() > 0.5f;
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (stopGame > 0) return;
        if (buildSystem.BuildModeOn > 0f) return;

        if (context.performed)
        {
            if(info.magazineCount > 0)
            {
                //������ ��� ����
                animator.SetBool(hashReload, true);                
            }            
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started) return;
        if (stopGame > 0) return;
        if (buildSystem.BuildModeOn > 0f)
        {
            if (context.performed)
            {
                isFire = true;
                buildSystem.BuildTurret();                
            }
            if (context.canceled)
            {
                isFire = false;
            }
        }
        else
        {
            if (context.performed)
            {
                isFire = true;
                //FireGun();
            }
            if (context.canceled)
            {
                isFire = false;
            }
        }                  
    }

    private void Update()
    {
        if (info.isDead) return;
        //GameStopping();

        //���� -> �ٷ� Ȱ��ȭ�Ұ���
        CameraRotation();

        AimingOnOff();
        AimingCamera();

    }

    private void FixedUpdate()
    {
        if (info.isDead) return;
        DecideRecoilBack();
        FireGun();

        AttackDelay();
        ChangeNotAimingDelay();
    }

    //�ʱ� ī�޶� ȸ�� ���� �̻��� ���� �� �ϳ��� ������
    private void CameraRotation()
    {
        if (GameManager.Instance.isGameStop > 0)
        {
            return;
        }
        x_Axis.Update(Time.fixedDeltaTime);
        y_Axis.Update(Time.fixedDeltaTime);

        mouseRotation = Quaternion.Euler(y_Axis.Value, x_Axis.Value, 0f);

        CameraLookAt.rotation = Quaternion.Lerp(CameraLookAt.rotation, mouseRotation, cameraLerftime);        
    }
    /*mouseRotation;*/
    //Quaternion.Lerp(CameraLookAt.rotation, mouseRotation, cameraLerfTime);

    //�ѱ� �ݹ� �� ī�޶� ��鸲
    // ���� �ݹ��ϸ� cameralerftime �� 
    private void DecideRecoilBack()
    {
        if (animator.GetBool(hashZoomOn))   //���� ��
        {
            recoilBackForce = aimingrecoilback;
        }
        else    //���� ���� �ƴ�
        {
            recoilBackForce = recoilback;
        }
    }

    private void FirearmRecoil()
    {       
        CameraLookAt.rotation = Quaternion.Euler(CameraLookAt.eulerAngles.x - recoilBackForce, CameraLookAt.eulerAngles.y, 0);
    }

    //private void GameStopping()
    //{
    //    if (isGameStop > 0)
    //    {
    //        Cursor.visible = true;
    //        Cursor.lockState = CursorLockMode.None;
    //        Time.timeScale = 0f;
    //    }
    //    else
    //    {
    //        Cursor.visible = false;
    //        Cursor.lockState = CursorLockMode.Locked;
    //        Time.timeScale = 1f;
    //    }
    //}

    private void AimingOnOff()
    {
        if (isAiming || notAimingTimer < notAimingDelayTime)
        {
            animator.SetBool(hashAiming, true);     
        }
        else
        {
            animator.SetBool(hashAiming, false);
            AttackTimer = AttackDelayTime;
        }
    }

    private void AimingCamera()
    {
        if (info.isDead) return;
        if (isAiming)
        {
            animator.SetBool(hashZoomOn, true);
        }
        else
        {
            animator.SetBool(hashZoomOn, false);
        }
    }

    private void ShootRay()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if(Physics.Raycast(ray, out RaycastHit hit, 30f, aimColliderLayerMask))
        {
            float errorFloat;
            if (!animator.GetBool(hashZoomOn)) errorFloat = attackAccuracy;
            else errorFloat = 0;

            float errorRange_x = Random.Range(-errorFloat, errorFloat);
            float errorRange_y = Random.Range(-errorFloat, errorFloat);
            float errorRange_z = Random.Range(-errorFloat, errorFloat);

            debugTransform.position = hit.point;

            if (Physics.Raycast(GunFireStartPoint.position, (hit.point - GunFireStartPoint.position + new Vector3(errorRange_x, errorRange_y, errorRange_z)).normalized, out RaycastHit hits, 30f, FireColliderLayerMask))
            {                
                Debug.DrawLine(GunFireStartPoint.position, hits.point, Color.red);

                Debug.Log(hits.transform.gameObject.name);
                //������ �ο�
                if (hits.transform.gameObject.layer == LayerMask.NameToLayer("Monster"))
                {
                    if (hits.transform.CompareTag("Monster"))
                    {
                        if (hits.transform.CompareTag("MonsterHead"))
                        {
                            hits.transform.GetComponent<Monster>().Hurt(info.Attack);
                        }

                        hits.transform.GetComponent<Monster>().Hurt(info.Attack);
                    }
                    else if (hits.transform.CompareTag("Boss"))
                    {
                        if (hits.transform.CompareTag("MonsterHead"))
                        {
                            hits.transform.GetComponent<BossMonster>().Hurt(info.Attack);
                        }
                        hits.transform.GetComponent<BossMonster>().Hurt(info.Attack);
                    }
                    BloodEffectManger.Instance.UsingEffect(hits.point);

                }
                else if (hits.transform.CompareTag("Barrel"))
                {
                    Debug.Log(hit.transform.gameObject.name);
                    hits.transform.GetComponent<Barrel>().Hurt();
                }
                else if (hits.transform.CompareTag("MonsterSpawner"))
                {
                    hits.transform.GetComponent<MonsterSpawnCrystal>().Hurt(info.Attack);
                }
            }

        }
    }

    private void FireGun()
    {
        if (buildSystem.BuildModeOn > 0f) return;
        if (animator.GetBool(hashReload)) return;
        
        if (isFire && AttackAble)
        {            
            AttackAble = false;
            AttackTimer = AttackDelayTime;
            notAimingTimer = 0;
            animator.SetBool(hashFire, true);
            cameraLerftime = 0.5f;
        }
        //else
        //{
        //    cameralerftime = 0.1f;
        //}
    }

    public void CameraLerftimeEnd()
    {
        cameraLerftime = 1f;
    }

    public void FireEnd()
    {        
        animator.SetBool(hashFire, false);
        cameraLerftime = 1f;
        //cameralerftime = 1f;
    }

    public void Fire()
    {        
        if(info.equipedBulletCount > 0)
        {            
            //������ �ο�
            ShootRay();

            ////ī�޶� �ݵ�
            FirearmRecoil();

            //���� ��ƼŬ ���
            ParticleSystem.GetComponent<ParticleSystem>().Play();

            //�ݹ� �Ҹ� ���
            audioSource.clip = shootAudioClip[0];
            audioSource.Play();

            //ź�� �� ����
            info.equipedBulletCount--;

            //UI �ݿ�
            UI.ChangeFireText(info.equipedBulletCount);
        }
        else
        {
            //�� źâ �Ҹ� ���
            audioSource.clip = shootAudioClip[1];
            audioSource.Play();
        }
    }


    public void ReloadEnd()
    {
        animator.SetBool(hashReload, false);
        notAimingTimer = 0;
        UI.Reload(info.equipedBulletCount, info.magazineCount);
        Debug.Log("���� ����...");
        Debug.Log($"ź �� : {info.equipedBulletCount}, źâ �� : {info.magazineCount}");
    }

    public void Reloading()
    {
        if(info.magazineCount > 0)
        {
            info.magazineCount--;
            info.equipedBulletCount = info.maxEquipedBulletCount;            
        }
    }

    public void ReloadSoundPlay()
    {
        //������ ����� ����
        audioSource.clip = shootAudioClip[2];
        audioSource.Play();
    }

    private void AttackDelay()
    {
        if(AttackTimer > 0)
        {
            AttackTimer -= Time.fixedDeltaTime;
        }
        else
        {
            AttackAble = true;
        }
    }

    private void ChangeNotAimingDelay()
    {
        if(notAimingTimer < notAimingDelayTime)
        {
            notAimingTimer += Time.fixedDeltaTime;
        }
    }
}
