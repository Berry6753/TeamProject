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

    //IEnumerator InputSelectAction()
    //{
    //    while (true)
    //    {
    //        commandQueue.Clear();

    //        isPush = false;

    //        yield return new WaitUntil(() => Input.anyKeyDown);
    //        if (Input.GetKeyDown(KeyCode.Alpha1))
    //        {
    //            Debug.Log("커멘드 명령");
    //            commandQueue.Enqueue(1);
    //        }
    //        else if (Input.GetKeyDown(KeyCode.Alpha2))
    //        {
    //            Debug.Log("Player Upgrade");
    //            commandQueue.Enqueue(2);
    //        }
    //        else if (Input.GetKeyDown(KeyCode.Alpha3))
    //        {
    //            Debug.Log("Core Upgrade");
    //            commandQueue.Enqueue(3);
    //        }
    //        else if (Input.GetKeyDown(KeyCode.Alpha4))
    //        {
    //            Debug.Log("탑 보급");
    //            commandQueue.Enqueue(4);
    //        }
    //        else if (Input.GetKeyDown(KeyCode.Alpha5))
    //        {
    //            Debug.Log("X");
    //            commandQueue.Enqueue(0);
    //        }

    //        isPush = true;
    //        yield return new WaitUntil(() => isPush);
            
    //        if(commandQueue.Count > 0)
    //        {
    //            SelectAction();                             
    //            yield break;
    //        }
    //    }
    //}

//    private void SelectAction()
//    {
//        switch(commandQueue.Dequeue())
//        {
//            case 1:
//                StartCoroutine(PushCommand());
//                break;
//            case 2:
//                StartCoroutine(UpgradePlayer());
//                break;
//            case 3:
//                CoreUpgrade();
//                commandQueue.Clear();
//                isCommand = false;
//                break;
//            case 4:
//                .Reloading();
//                commandQueue.Clear();
//                isCommand = false;
//                break;
//            default:
//                Debug.Log("커멘트 종료");
//                commandQueue.Clear();
//                isCommand = false;
//                break;
//        }
//    }

//    //코어 업그레이드
//    private void CoreUpgrade()
//    {
//        if (info.GearCount < core.upgradeCost)
//            core.isUpgrading = true;
//    }

//    //Player Upgrade
//    IEnumerator UpgradePlayer()
//    {
//        while (true)
//        {
//            commandQueue.Clear();

//            isPush = false;

//            yield return new WaitUntil(() => Input.anyKeyDown);
//            if (Input.GetKeyDown(KeyCode.Alpha1))
//            {
//                Debug.Log("업그레이드");
//                commandQueue.Enqueue(1);
//            }
//            else if (Input.GetKeyDown(KeyCode.Alpha2))
//            {
//                Debug.Log("X");
//                commandQueue.Enqueue(0);
//            }

//            isPush = true;
//            yield return new WaitUntil(() => isPush);

//            if (commandQueue.Count <= 0) continue;

//            switch (commandQueue.Dequeue())
//            {
//                case 1:
//                    //코스트가 부족하면 continue;
//                    if (info.GearCount < LevelUpCost)
//                    {
//                        Debug.Log("기어 부족");
//                        continue;
//                    }
//                    //코스트 소비
//                    info.UseGear((int)Mathf.Round(LevelUpCost));
//                    //Player 스텟 증가
//                    info.PlayerUpgradeComplete();
//                    //필요 코스트 증가
//                    LevelUpCost *= upCostValue;

//                    commandQueue.Clear();

//                    Debug.Log("종료하지 전까지 계속 지속됨");
//                    yield return true;
//                    break;
//                default:
//                    //닫기
//                    Debug.Log("커멘트 종료");
//                    commandQueue.Clear();
//                    //isCommand = false;
//                    yield break;
//            }
//        }
//    }

//    // Skill Item Command 입력
//    IEnumerator PushCommand()
//    {
//        commandQueue.Clear();

//        while (true)
//        {
//            isPush = false;

//            yield return new WaitUntil(() => Input.anyKeyDown);
//            if (Input.GetKeyDown(KeyCode.W))
//            {
//                commandQueue.Enqueue(1);
//            }
//            else if (Input.GetKeyDown(KeyCode.A))
//            {
//                commandQueue.Enqueue(2);
//            }
//            else if (Input.GetKeyDown(KeyCode.S))
//            {
//                commandQueue.Enqueue(3);
//            }
//            else if (Input.GetKeyDown(KeyCode.D))
//            {
//                commandQueue.Enqueue(4);
//            }
//            else
//            {
//                Debug.Log("X");
//                commandQueue.Enqueue(0);
//            }

//            isPush = true;
//            yield return new WaitUntil(() => isPush);
        
//            if (commandQueue.Count == 4)
//            {
//                if (Core.instance.CheckeCommand())
//                {
//                    commandQueue.Clear();
//                    //커멘드 모드 OFF
//                    isCommand = false;
//                    yield break;
//                }
//                else
//                {
//                    commandQueue.Clear();
//                }
                
//                //StopCoroutine(PushCommand());
//            }

//        }
//    }
//}
