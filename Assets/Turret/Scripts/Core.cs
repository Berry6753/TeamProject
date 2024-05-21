using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum PlayerSkillName
{
    BARRICATE,
    HEALLING,
    NUKE,

    LAST
}


public class Core : MonoBehaviour
{
    public static Core instance;

    public int repairCost = 100;
    public int upgradeCost = 50;
    public bool isUpgrade = true;
    public bool isUpgrading = false;
    public bool isReloading = true;

    [SerializeField]
    private GameObject itemSpawnPos;

    private int upgradeCoolTimeRise = 10;
    private int nowHp;
    private int maxHp = 100;
    private int hpRise = 30;
    private int realoadCoolTime = 30;
    private int upgradeCoolTime = 30;
    private int upgradeCostRise = 2;
    private int nowUpgradeCount;
    private int maxUpgradeCount = 5;
    private float checkReloadingTime;
    private float checkUpgradeTime;
    private int checkCount;
    public Player_Info player;
    public Player_Command player_Command;
    public bool isPlayer;

    private Dictionary<int, int[]> commandDic = new Dictionary<int, int[]>();
    [SerializeField]
    private GameObject[] skillObj;

    private Queue<GameObject>[] skillObjQue = new Queue<GameObject>[(int)PlayerSkillName.LAST];

    [Header("Core 체력 UI")]
    [SerializeField]
    private Image CoreHPBar;

    [Header("Core 체력 Text")]
    [SerializeField]
    private TMP_Text CoreHPText;

    [Header("쿨타임 확인용 Icon")]
    [SerializeField]
    private Image ReloadCoolTimeIcon;

    [SerializeField]
    private Canvas tabUI;
    public float upgradeTime { get { return checkUpgradeTime; } }


    private int itemKey;

    private void Awake()
    {
        instance = this;
        tabUI.enabled = false;
        //for (int i = 0; i < (int)PlayerSkillName.LAST; i++)
        //{
        //    skillObjQue[i] = new Queue<GameObject>();
        //    for (int j = 0; j < 10; j++)
        //    {
        //        GameObject gameObject = Instantiate(skillObj[i]);
        //        skillObjQue[i].Enqueue(gameObject);
        //        gameObject.SetActive(false);

        //    }
        //}
    }

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Player_Info>();
        player_Command = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Player_Command>();
        checkReloadingTime = realoadCoolTime;
        ReloadCoolTimeIcon.fillAmount = 0;
        CoreHPBar.fillAmount = 1;
        InitCommandDic();
    }

    // Start is called before the first frame update
    void Start()
    {
        nowHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isReloading)
        {
            checkReloadingTime -= Time.deltaTime;            
            if (checkReloadingTime <= 0)
            {
                isReloading = true;
                checkReloadingTime = realoadCoolTime;
                ReloadCoolTimeIcon.fillAmount = 0;
            }
            else ReloadCoolTimeIcon.fillAmount = checkReloadingTime / realoadCoolTime;            
        }

        if (isUpgrading)
        {
            checkUpgradeTime += Time.deltaTime;
            if (checkUpgradeTime >= upgradeCoolTime)
            {
                Upgrade();
                checkUpgradeTime = 0;
                isUpgrading = false;
            }
        }

        if (nowUpgradeCount >= maxUpgradeCount)
        {
            isUpgrade = false;
        }

        if (isPlayer)
        {
            tabUI.enabled = true;
        }
        else
        {
            tabUI.enabled = false;
        }
        //파괴될시 게임매니저의 코어가있음을 뜻하는 변수를 false로
    }

    public void Reloading()
    {
        if (isReloading && player != null)
        {
            //플레이어의 탄창수 채워줌
            player.equipedBulletCount = player.maxEquipedBulletCount;
            player.magazineCount = player.maxMagazineCount;
            isReloading = false;
            player.GetComponent<Player_Info_UI>().Reload(player.equipedBulletCount, player.magazineCount);
        }
    }

    public void Hurt(int damge)
    {
        nowHp -= damge;
        CoreHPBar.fillAmount = nowHp / maxHp;
        CoreHPText.text = $"{nowHp} / {maxHp}";
    }

    public void Repair()
    {
        nowHp = maxHp;
        CoreHPBar.fillAmount = nowHp / maxHp;
        CoreHPText.text = $"{nowHp} / {maxHp}";
    }

    public void Upgrade()
    {
        nowHp += hpRise;
        maxHp += hpRise;
        upgradeCost *= upgradeCostRise;
        upgradeCoolTime += upgradeCoolTimeRise;
        nowUpgradeCount++;
    }

    private void InitCommandDic()
    {
        commandDic.Add((int)PlayerSkillName.BARRICATE, new int[4] { 2, 4, 2, 4 });
        commandDic.Add((int)PlayerSkillName.HEALLING, new int[4] { 2, 2, 2, 1 });
        commandDic.Add((int)PlayerSkillName.NUKE, new int[4] { 1, 1, 1, 2 });
        
    }

    public bool CheckeCommand()
    {
        foreach (int i in commandDic.Keys)
        {
            commandDic.TryGetValue(i, out int[] Value);
            if (!Enumerable.SequenceEqual(Value, player_Command.commandQueue.ToArray()))
            {
                itemKey = -1;
            }
            else
            {
                itemKey = i;
                break;
            }
            //for (int j = 0; j < Value.Length; j++) 
            //{
                
            //    if(!(Value[j] == playerMovement.commandQueue.ToArray()[j]))
            //    {
            //        itemKey = -1;
            //        checkCount = 0;
            //        break;
            //    }
            //    else
            //    {
            //        checkCount++;
                    
            //    }

               

            //}
            //if (checkCount == 4)
            //{
            //    itemKey = i;
            //    checkCount = 0;
            //    break;
            //}
            //if (playerMovement.commandQueue.ToArray()==Value)
            //{
            //    itemKey = i;
            //    Debug.Log(itemKey + "asdad");
            //    break;
            //}
            //else
            //{
            //    Debug.Log((playerMovement.commandQueue.ToArray() == Value) + "gggggggggg");
            //    for (int j = 0; j < Value.Length; j++)
            //    {
            //        Debug.Log(Value[j] + $"aaa{j}aaa");
            //        Debug.Log(playerMovement.commandQueue.ToArray()[j] + $"sssss{j}sssss");

            //    }
            //    Debug.Log(itemKey + "a");
                
            //}
        }

        if (itemKey == -1 || itemKey == (int)PlayerSkillName.LAST)
        {
            Debug.Log("커맨드 틀림");
            return false;
        }
        else
        {            
            GameObject gameObject = ItemObjectPool.SpawnFromPool(skillObj[itemKey].name, itemSpawnPos.transform.position);
            //GameObject gameObject = skillObjQue[itemKey].Dequeue();
            //skillObjQue[itemKey].Enqueue(gameObject);
            //gameObject.SetActive(true);
            //gameObject.transform.position = itemSpawnPos.transform.position;
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 10, 10));
            player.UseGear(gameObject.GetComponent<Skill_Item_Info>().Count);
            return true;
        }
    }

}
