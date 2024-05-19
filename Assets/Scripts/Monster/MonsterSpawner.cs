using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;    //스폰위치
    private Wave currentWave;

    private List<GameObject> monsterList;
    public List<GameObject> MonsterList => monsterList;

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
                GameObject clone = Instantiate(currentWave.monsterPrefab[i]);
                GameObject monster = clone.GetComponent<GameObject>();

                monsterList.Add(monster);

                spawnMonsterCount[i]++;

                yield return new WaitForSeconds(currentWave.spawnTime);
            }
            yield return new WaitForSeconds(currentWave.spawnTime);
        }
    }
}
