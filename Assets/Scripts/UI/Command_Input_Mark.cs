using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Command_Input_Mark : MonoBehaviour
{
    private Core core;

    private Player_Command player_Command;

    [SerializeField] private Image[] inputImage;
    [SerializeField] private Sprite[] directImage;
    [SerializeField] private Sprite xImage;
    [SerializeField] private Button button;
    [SerializeField] private Button exit;
    private bool isPush;

    [Header("커맨드 입력 값의 길이")]
    [SerializeField]
    private int InputCommandLength;

    public Queue<int> commandQueue;

    private void Awake()
    {
        core = GameManager.Instance.GetCore.GetComponent<Core>();
        player_Command = GameManager.Instance.GetPlayer.GetComponent<Player_Command>();
    }

    private void OnEnable()
    {
        foreach (Image item in inputImage)
        {
            item.sprite = null;
        }
        isPush = false;
    }
    public void CommandMark()
    {
        StartCoroutine(OnCommandMark());
    }

    //Skill Item Command 입력
    //IEnumerator PushCommand()
    //{
    //    commandQueue.Clear();

    //    int Count = 0;
    //    while (true)
    //    {
    //        isPush = false;
    //        yield return new WaitUntil(() => Input.anyKeyDown);
    //        if (Input.GetKeyDown(KeyCode.W))
    //        {
    //            commandQueue.Enqueue(1);
    //            inputImage[Count].sprite = directImage[0];
    //        }
    //        else if (Input.GetKeyDown(KeyCode.A))
    //        {
    //            commandQueue.Enqueue(2);
    //            inputImage[Count].sprite = directImage[1];
    //        }
    //        else if (Input.GetKeyDown(KeyCode.S))
    //        {
    //            commandQueue.Enqueue(3);
    //            inputImage[Count].sprite = directImage[2];
    //        }
    //        else if (Input.GetKeyDown(KeyCode.D))
    //        {
    //            commandQueue.Enqueue(4);
    //            inputImage[Count].sprite = directImage[3];
    //        }
    //        else
    //        {
    //            Debug.Log("X");
    //            commandQueue.Enqueue(0);
    //            inputImage[Count].sprite = xImage;
    //        }

    //        isPush = true;
    //        yield return new WaitUntil(() => isPush);

    //        if (commandQueue.Count == 4)
    //        {
    //            if (core.CheckeCommand())
    //            {
    //                commandQueue.Clear();

    //                //커멘드 모드 OFF
    //                //isCommand = false;
    //                yield break;
    //            }
    //            else
    //            {
    //                commandQueue.Clear();
    //                Count = 0;
    //            }

    //            //StopCoroutine(PushCommand());
    //        }

    //    }
    //}

    private IEnumerator OnCommandMark()
    {
        int Count = 0;
        commandQueue.Clear();
        while (Count < InputCommandLength)
        {
            isPush = true;
            yield return new WaitUntil(() => isPush);
            yield return new WaitUntil(() => Input.anyKeyDown);
            if (Input.GetKeyDown(KeyCode.W))
            {
                commandQueue.Enqueue(1);
                inputImage[Count].sprite = directImage[0];
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                commandQueue.Enqueue(2);
                inputImage[Count].sprite = directImage[1];
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                commandQueue.Enqueue(3);
                inputImage[Count].sprite = directImage[2];
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                commandQueue.Enqueue(4);
                inputImage[Count].sprite = directImage[3];
            }
            else
            {
                commandQueue.Enqueue(0);
                inputImage[Count].sprite = xImage;
            }
            Count++;
            isPush = false;
            yield return new WaitUntil(() => !isPush);
            Debug.Log("c"+Count);
        }

        if (Count >= InputCommandLength)
        {
            yield return new WaitUntil(() => Input.anyKeyDown);
            if (Input.GetKeyDown(KeyCode.Return))
            {
                button.interactable = true;
                exit.interactable = true;
                Focus(button);

                if (core.CheckeCommand())
                {
                    commandQueue.Clear();

                    //커맨드 종료
                    player_Command.isCommand = false;
                    //yield break;
                }
                else
                {
                    commandQueue.Clear();
                    Count = 0;
                }



                //이미지 초기화////////////                    
                foreach (Image item in inputImage)
                {
                    item.sprite = null;
                }
                //////////////////////////////////
                Debug.Log("a" + button.interactable);
                yield break;
            }           
        }

        //while (true)
        //{

        //}
    }

    private void Focus(Button button)
    {
        EventSystem.current.SetSelectedGameObject(button.gameObject);
    }

}
