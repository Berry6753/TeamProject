using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;

    [Header("카메라")]
    [SerializeField]
    private CinemachineFreeLook cameraTransform;

    [Header("카메라 LookAt")]
    [SerializeField]
    private Transform cameraLookAt;

    private Vector2 InputDir;
    private Vector3 inputMoveDir;
    private Vector3 playerMoveDir;

    [Space(10)]
    [Header("이동 속도")]
    [SerializeField]
    private float moveSpeed;

   // private Animator animator;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        //animator = GetComponent<Animator>();
    }

    private void Start()
    {
        //if (isLocalPlayer == false) return;
        cameraTransform = GameObject.FindWithTag("PlayerCamera").GetComponent<CinemachineFreeLook>();

        cameraTransform.Follow = this.transform;
        cameraTransform.LookAt = cameraLookAt;
    }

    // Update is called once per frame
    void Update()
    {
        Rotation();
        Movement();

        //Debug.Log(cameraTransform.rotation);
        //Debug.Log(transform.forward);
    }

    // InputSystem 무브먼트
    private void OnMovement(InputValue inputValue)
    {
        InputDir = inputValue.Get<Vector2>();
    }

    private void Movement()
    {
        inputMoveDir = new Vector3(InputDir.x, 0, InputDir.y).normalized;

        if (inputMoveDir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputMoveDir.x, inputMoveDir.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            playerMoveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            characterController.Move(playerMoveDir.normalized * moveSpeed * Time.deltaTime);

        }

        //animator.SetBool("Move", inputMoveDir.magnitude >= 0.1f);
    }

    private void Rotation()
    {
        // 카메라의 방향을 캐릭터의 회전값으로 변환
        Quaternion targetRotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);

        // 캐릭터를 해당 회전값으로 회전시킴
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.8f);

    }
}
