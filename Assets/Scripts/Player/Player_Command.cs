using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Command : MonoBehaviour
{
    [Header("Player LevelUp Cost �ʱⰪ")]
    [SerializeField]
    private float LevelUpCost;

    [Header("LevelUp Cost ���� ��ġ(���� : n�� )")]
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

                Debug.Log("Ŀ��� ON");

                //player UI off
                GameManager.Instance.GetPlayerUI.SetActive(false);

                //Core ī�޶� �ִϸ��̼� ����
                Camera.main.cullingMask = commandModeCameraLayerMask;
                coreCamera.enabled = true;
                coreCamera.Priority = 11;
                coreAnimator.SetBool(hashCommand, true);

                //�ִϸ��̼� �̺�Ʈ�� ó��
                // Command UI On
                //GameManager.Instance.GetCoreUI.SetActive(true);
                //GameManager.Instance.isGameStop = 1f;
            }
        }
    }
}
