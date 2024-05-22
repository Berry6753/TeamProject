using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoint;    //스폰위치
    private Wave currentWave;

    private List<GameObject> monsterList;
    public List<GameObject> MonsterList => monsterList;
    private int randomS;

    private void Awake()
    {
        monsterList = new List<GameObject>();
    }

    public void StartWave(Wave wave)
    {
        currentWave = wave;

        StartCoroutine(SpawnMonster());
    }
        
    private IEnumerator SpawnMonster()
    {
        int[] spawnMonsterCount = new int[currentWave.maxMonsterCount.Length];

        for (int i = 0; i < spawnMonsterCount.Length; i++)
        {
            while (spawnMonsterCount[i] < currentWave.maxMonsterCount[i])
            {
                GameObject clone = MonsterObjectPool.SpawnFromPool(currentWave.monsterPrefab[i].GetComponent<Monster>().GetMonsterName, spawnPoint[RandomSpawn()].position);
                
                /*Instantiate(currentWave.monsterPrefab[i], spawnPoint[RandomSpawn()]);*/
                Monster monster = clone.GetComponent<Monster>();

                monsterList.Add(clone);

                monster.OnDeath += HandleMosnterDeath;

                spawnMonsterCount[i]++;

                yield return new WaitForSeconds(currentWave.spawnTime);
            }
            yield return new WaitForSeconds(currentWave.spawnTime);
        }
    }

    private int RandomSpawn()
    {
        for (int i = 0; i < spawnPoint.Length; i++)
        {
            randomS = Random.Range(0, spawnPoint.Length);
        }
        return randomS;
    }

    private void HandleMosnterDeath(Monster monster)
    {
        monsterList.Remove(monster.gameObject);
    }
}
