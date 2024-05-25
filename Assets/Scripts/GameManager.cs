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

    [Header("PlayerSpawnPoint")]
    [SerializeField]
    private Transform spawnPoint;

    [Header("시작 메뉴")]
    [SerializeField] private GameObject startUI;

    [Header("Pause UI")]
    [SerializeField] private GameObject pauseUI;

    [Header("사운드 매니저")]
    [SerializeField] private GameObject sound;

    [Header("보스")]
    [SerializeField] private BossMonster boss;

    [Header("")]
    public float isGameStop;

    public GameObject GetPlayer {  get { return Player; } }
    public GameObject GetCore {  get { return core; } }
    public GameObject GetCoreUI { get {  return coreUI; } }
    public GameObject GetPlayerUI { get { return playerUI; } }
    public Transform GetSpawnPoint {  get { return spawnPoint; } }
    public GameObject Sound { get { return sound; } }
    public BossMonster Boss { get { return boss; } }



    private void Awake()
    {
        isGameStop = 1;
    }

    public void OnGameStop(InputAction.CallbackContext context)
    {
        if (startUI.activeSelf == true) return;
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

    #region Exit

    public void ExitGame()
    {
        Application.Quit();
    }

    #endregion
}
