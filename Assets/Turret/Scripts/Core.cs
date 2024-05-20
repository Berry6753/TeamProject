using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;


public enum PlayerSkillName
{
    NUKE,

    LAST
}


public class Core : MonoBehaviour
{

    public int repairCost = 100;
    public int upgradeCost = 50;
    public bool isUpgrade = true;
    public bool isUpgrading = false;
    public bool isReloading = false;

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
    public Player_Info player;
    public PlayerMovement playerMovement;
    public bool isPlayer;

    private Dictionary<int, int[]> commandDic = new Dictionary<int, int[]>();
    [SerializeField]
    private GameObject[] skillObj;

    private Queue<GameObject>[] skillObjQue = new Queue<GameObject>[(int)PlayerSkillName.LAST];




    private int itemKey;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Player_Info>();
        playerMovement=GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerMovement>();
        InitCommandDic();
        for (int i = 0; i < (int)PlayerSkillName.LAST; i++)
        {
            skillObjQue[i] = new Queue<GameObject>();
            for (int j = 0; j < 10; j++)
            {
                GameObject gameObject = Instantiate(skillObj[i]);
                skillObjQue[i].Enqueue(gameObject);
                gameObject.SetActive(false);

            }
        }
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
            checkReloadingTime += Time.deltaTime;
            if (checkReloadingTime >= realoadCoolTime)
            {
                isReloading = true;
                checkReloadingTime = 0;
            }
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

        }


    }

    public void Hurt(int damge)
    {
        nowHp -= damge;
    }

    public void Repair()
    {
        nowHp = maxHp;
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
        commandDic.Add((int)PlayerSkillName.NUKE, new int[4] { 1, 1, 1, 2 });
        
    }

    public void CheckeCommand()
    {
        foreach (int i in commandDic.Keys)
        {
            commandDic.TryGetValue(i, out int[] Value);
            for (int j = 0; j < Value.Length; j++)
            {
                if(!(Value[j] == playerMovement.commandQueue.ToArray()[j]))
                {
                    itemKey = -1;
                    break;
                }
                else
                {
                    itemKey = i;
                }

            }
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
        }
        else
        {
            GameObject gameObject = skillObjQue[itemKey].Dequeue();
            skillObjQue[itemKey].Enqueue(gameObject);
            gameObject.SetActive(true);
            gameObject.transform.position = itemSpawnPos.transform.position;
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 10, 10));
        }
    }

}
