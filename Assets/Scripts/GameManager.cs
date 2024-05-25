using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using Unity.VisualScripting;
using TMPro;
using Newtonsoft.Json;
using System.Linq;

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

    [Header("WaveSystem")]
    [SerializeField]
    private WaveSystem waveSystem;

    [Header("Record Text")]
    [SerializeField]
    private List<TMP_Text> RecordText;

    [Header("BossMonster")]
    [SerializeField]
    private BossMonster boss;

    [Header("Sound Manager")]
    [SerializeField] private GameObject sound;

    public float isGameStop;
    public bool isGameOver { get; private set; }
    private Player_Info playerInfo;
    private Player_Command Player_Command;

    public GameObject GetPlayer {  get { return Player; } }
    public GameObject GetCore {  get { return core; } }
    public GameObject GetCoreUI { get {  return coreUI; } }
    public GameObject GetPlayerUI { get { return playerUI; } }
    public Transform GetSpawnPoint {  get { return spawnPoint; } }
    public WaveSystem WaveSystem { get { return waveSystem; } }
    public BossMonster Boss { get { return boss; } }
    public GameObject Sound { get { return sound; } }

    private void Awake()
    {
        isGameStop = 1;
        isGameOver = false;
        playerInfo = Player.GetComponent<Player_Info>();
        Player_Command = Player.GetComponent<Player_Command>();
    }

    public void OnGameStop(InputAction.CallbackContext context)
    {
        if (startUI.activeSelf == true) return;
        if (context.started) return;
        if (context.performed)
        {
            if (Player_Command.isCommand) return;
            isGameStop *= -1;
        }        
    }

    public void OnGameStart()
    {
        isGameStop = -1;
    }

    private void Update()
    {
        GameStopping();

        //if (isGameOver)
        //{
        //    //데이터 저장
        //    SaveDataToJson();

        //    //게임 오버 화면

        //}
    }

    public void ShowRecord()
    {
        LoadRankingData();

        int index = 0;
        foreach (var texts in RecordText)
        {
            if (dataList.Count < index+1) continue;
            texts.text = $"Wave : {dataList[index].waveCount}, timer : {dataList[index].playTime}";
            index++;
        }
    }

    private void Start()
    {
        path = Path.Combine(Application.persistentDataPath, "waveData.json");

        LoadRankingData();
    }

    string path;
    
    class SaveData
    {
        public int waveCount;
        public float playTime;
    }
    
    List<SaveData> dataList = new List<SaveData>();

    public void SaveDataToJson()
    {
        if(File.Exists(path))
        {
            LoadRankingData();
        }

        SaveData data = new SaveData();
        data.waveCount = waveSystem.waveCount;
        data.playTime = waveSystem.PlayTimer;

        dataList.Add(data);
        dataList = dataList.OrderByDescending(x => x.waveCount).ThenBy(x => x.playTime).ToList();

        Debug.Log(dataList.Count);

        string jsonData = JsonConvert.SerializeObject(dataList);/*JsonUtility.ToJson(dataList);*/

        File.WriteAllText(path, jsonData);
    }

    public void LoadRankingData()
    {
        if(!File.Exists(path))
        {
            SaveDataToJson();
        }

        string jsonData = File.ReadAllText(path);
        dataList = JsonConvert.DeserializeObject<List<SaveData>>(jsonData);/*JsonUtility.FromJson<List<SaveData>>(jsonData);*/
        

        //int index = 0;
        //foreach(var texts in RecordText)
        //{
        //    texts.text = $"{dataList[index]}";
        //    index++;
        //}
    }

    private void GameStopping()
    {
        if (isGameStop > 0)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            if (!Player_Command.isCommand)
            {
                pauseUI.SetActive(true);
            }            
            Time.timeScale = 0f;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            if (!Player_Command.isCommand)
            {
                pauseUI.SetActive(false);
            }
            Time.timeScale = 1f;
        }
    }

    #region Exit

    public void ExitGame()
    {
        Application.Quit();
    }

    #endregion

    private void CheckGameOver()
    {
        if(!core.activeSelf && playerInfo.isDead)
        {
            isGameOver = true;
        }
    }

}
