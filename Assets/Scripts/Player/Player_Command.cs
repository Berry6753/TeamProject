using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Command : MonoBehaviour
{
    [Header("Player LevelUp Cost 초기값")]
    [SerializeField]
    private float LevelUpCost;

    [Header("LevelUp Cost 증가 수치(기준 : n배 )")]
    [SerializeField]
    private float upCostValue;

    public bool isCommand;
    public bool isCore;

    private Core core;
    private Animator coreAnimator;
    private CinemachineVirtualCamera coreCamera;

    [SerializeField]
    private LayerMask commandModeCameraLayerMask;

    public LayerMask defualtMask { get; private set; }

    private readonly int hashCommand = Animator.StringToHash("Zoom");

    private void Awake()
    {
        core = GameManager.Instance.GetCore.GetComponent<Core>();
        coreAnimator = core.GetComponent<Animator>();
        coreCamera = core.GetComponentInChildren<CinemachineVirtualCamera>();

        defualtMask = Camera.main.cullingMask;
    }

    public void OnCommand(InputAction.CallbackContext context)
    {
        if (context.started) return;
        if (context.performed)
        {
            if (isCore && !isCommand)
            {
                isCommand = true;

                Debug.Log("커멘드 ON");

                //player UI off
                GameManager.Instance.GetPlayerUI.SetActive(false);

                //Core 카메라 애니메이션 실행
                Camera.main.cullingMask = commandModeCameraLayerMask;
                coreCamera.enabled = true;
                coreCamera.Priority = 11;
                coreAnimator.SetBool(hashCommand, true);

                //애니메이션 이벤트로 처리
                // Command UI On
                //GameManager.Instance.GetCoreUI.SetActive(true);
                //GameManager.Instance.isGameStop = 1f;
            }
        }
    }
}
