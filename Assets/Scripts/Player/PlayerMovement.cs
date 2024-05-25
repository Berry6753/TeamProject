using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    private Player_Aiming aiming;
    private Player_Command command;
    private Player_Info info;
    private CharacterController characterController;

    private Quaternion startRotation;

    [Header("카메라")]
    [SerializeField]
    private Transform followcamera;

    private CinemachineVirtualCamera virtualCamera;

    [Header("카메라 LookAt")]
    [SerializeField]
    private Transform cameraLookAt;

    private Vector2 InputDir;
    private Vector3 inputMoveDir;
    private Vector3 playerMoveDir;

    public bool InputBool {  get; private set; }
    //private bool isRunning;

    [Space(10)]
    [Header("그라운드 확인 overlap")]
    [SerializeField]
    private Transform overlapPos;

    [Space(10)]
    [Header("점프 Power")]
    [SerializeField]
    private float JumpPower;

    [Space(10)]
    [Header("중력 가속도")]
    [SerializeField]
    private float gravityMultiplier;

    [Header("걷는 소리 모음")]
    [SerializeField]
    private List<AudioClip> walkAudio;

    private float moveSpeed;

    //중력 값
    private float gravity = -9.81f;
    //현재 중력 가속도
    private float _velocity;

    private Animator animator;
    private float animationFloat;

    private float targetAngle;

    // private Animator animator;

    [Space(10)]
    [Header("캐릭터 회전 속도")]
    [SerializeField]
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    private readonly int hashMoveZ = Animator.StringToHash("Z_Speed");
    private readonly int hashMoveX = Animator.StringToHash("X_Speed");
    private readonly int hashFire = Animator.StringToHash("Fire");

    [SerializeField]
    private LayerMask gravityLayermask;

    //지반 확인용 overlapCollider
    private Collider[] colliders;

    private bool isGround;

    private float stopGame;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        aiming = GetComponent<Player_Aiming>();
        info = GetComponent<Player_Info>();
        command = GetComponent<Player_Command>();

        _velocity = 0f;

        virtualCamera = followcamera.GetComponent<CinemachineVirtualCamera>();
        stopGame = GameManager.Instance.isGameStop;
        //startRotation = transform.rotation;
        //Camera.main.transform.rotation = startRotation;
    }

    private void OnEnable()
    {
        //transform.rotation = startRotation;

        //cameraLookAt.rotation = startRotation;  
        //Camera.main.transform.rotation = startRotation;
        //virtualCamera.transform.rotation = startRotation;

        //virtualCamera.transform.position = transform.position - transform.forward * 3.5f;
        //Camera.main.transform.position = virtualCamera.transform.position;
        //virtualCamera.transform.rotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        Gravity();
        ChangeSpeed();
        Rotation();
    }

    private void FixedUpdate()
    {
        if (command.isCommand) return;
        Movement();
    }

    // InputSystem 무브먼트
    public void OnMovement(InputAction.CallbackContext context)
    {
        if (info.isDead) return;
        if (stopGame > 0) return;
        InputDir = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (info.isDead) return;
        if (stopGame > 0) return;
        InputBool = context.ReadValue<float>() > 0.5f;
    }

    //점프 버벅 거림 픽스 완료
    public void OnJump(InputAction.CallbackContext context)
    {
        if (info.isDead) return;
        if (stopGame > 0) return;
        if (colliders.Length <= 0) return;
        if (context.performed)
        {
            _velocity = Mathf.Sqrt(JumpPower * -1f * gravity);            
        }
    }


    ////기즈모를 활용하여 Overlap 육안으로 보기
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.black;
    //    Gizmos.DrawCube(overlapPos.position, new Vector3(0.3f, 0.1f, 0.3f));
    //}

    private void Gravity()
    {
        colliders = Physics.OverlapBox(overlapPos.position, new Vector3(0.3f,0.1f,0.3f), Quaternion.identity, gravityLayermask);

        if (colliders.Length > 0 && _velocity <= 0.0f)
        {
            _velocity = -1f;
            isGround = true;
        }
        else
        {
            _velocity += gravity * Time.deltaTime;
            isGround = false;
        }

        characterController.Move(new Vector3(0,_velocity,0) * Time.deltaTime);
    }

    private void Movement()
    {
        //if (!isGround) return;
        if (info.isDead)
        {
            InputDir = Vector3.zero;
            return;
        }
        inputMoveDir = new Vector3(InputDir.x, 0, InputDir.y).normalized;

        if (inputMoveDir.magnitude >= 0.1f)
        {            
            targetAngle = Mathf.Atan2(inputMoveDir.x, inputMoveDir.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            playerMoveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            characterController.Move(playerMoveDir.normalized * moveSpeed * Time.deltaTime);
        }

        MoveAnimation();
    }

    private void ChangeSpeed()
    {
        if (InputBool)
        {
            moveSpeed = info.runSpeed;
        }
        else
        {
            moveSpeed = info.defaultSpeed;
        }
    }

    private void MoveAnimation()
    {
        if(inputMoveDir.magnitude < 0.1f)
        {
            animationFloat = 0;
        }
        else if(InputBool)
        {
            animationFloat = 5;
        }
        else
        {
            animationFloat = 3;
        }

        animator.SetFloat(hashMoveX, Mathf.Lerp(animator.GetFloat(hashMoveX), InputDir.y * animationFloat, 10f));
        animator.SetFloat(hashMoveZ, Mathf.Lerp(animator.GetFloat(hashMoveZ), InputDir.x * animationFloat, 10f));
    }

    private void Rotation()
    {
        if (info.isDead) return;
        if (GameManager.Instance.isGameStop > 0) return;
        if (stopGame > 0)
        {
            Vector3 cameraPos = Camera.main.transform.position;
            Quaternion cameraRotation = Camera.main.transform.rotation;

            virtualCamera.Follow = null;

            virtualCamera.transform.position = cameraPos;
            virtualCamera.transform.rotation = cameraRotation;

            return;
        }
        else
        {
            if (virtualCamera.Follow == null)
            {
                virtualCamera.Follow = cameraLookAt;
            }

            // 카메라의 방향을 캐릭터의 회전값으로 변환
            Quaternion targetRotation = Quaternion.Euler(transform.rotation.x, Camera.main.transform.eulerAngles.y, transform.rotation.z);

            // 캐릭터를 해당 회전값으로 회전시킴
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
        }
    }    
}
