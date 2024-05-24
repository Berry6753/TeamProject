using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
    [Header("Player")]
    [SerializeField]
    private GameObject Player;

    [Header("Player")]
    [SerializeField]
    private GameObject core;

    [Header("Player_UI")]
    [SerializeField]
    private GameObject playerUI;

    [Header("Command_UI")]
    [SerializeField]
    private GameObject coreUI;

    public float isGameStop;

    [Header("½ÃÀÛ¸Þ´º")]
    [SerializeField] private GameObject StartUI;

    [Header("¸ØÃèÀ» ¶§ UI")]
    [SerializeField] private GameObject pauseUI;

    public GameObject GetPlayer {  get { return Player; } }
    public GameObject GetCore {  get { return core; } }
    public GameObject GetCoreUI { get {  return coreUI; } }
    public GameObject GetPlayerUI { get { return playerUI; } }

    private void Awake()
    {
        isGameStop = 1;
    }

    public void OnGameStop(InputAction.CallbackContext context)
    {
        if (StartUI.activeSelf == true) return;
        else isGameStop *= -1;
    }

    public void OnGameStart()
    {
        isGameStop = -1;
    }

    private void Update()
    {
        GameStopping();
    }

    private void GameStopping()
    {
        if (isGameStop > 0)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            pauseUI.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            pauseUI.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
