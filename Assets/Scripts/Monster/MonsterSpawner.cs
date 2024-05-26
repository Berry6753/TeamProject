using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MonsterSpawner : MonoBehaviour
{
    [Header("스폰 위치")]
    [SerializeField] private List<Transform> spawnPoint;    //스폰위치

    [Header("스폰 범위")]
    [SerializeField] private float spawnRange;    //스폰 범위
    private Wave currentWave;

    private List<GameObject> monsterList;
    public List<GameObject> MonsterList => monsterList;
    private int randomS;

    private void Awake()
    {
        monsterList = new List<GameObject>();
    }

    public void DestorySpawner(Transform spawner)
    {
        if (spawnPoint.Contains(spawner))
        {
            spawnPoint.Remove(spawner);
        }
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
                Vector3 monsterSpawnPoint = RandomSpawnPoint(spawnPoint[RandomSpawn()].position, spawnRange);
                GameObject clone = MonsterObjectPool.SpawnFromPool(currentWave.monsterPrefab[i].GetComponent<Monster>().GetMonsterName, monsterSpawnPoint/*spawnPoint[RandomSpawn()].position*/);
                
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
        for (int i = 0; i < spawnPoint.Count; i++)
        {
            randomS = Random.Range(0, spawnPoint.Count);
        }
        return randomS;
    }

    private Vector3 RandomSpawnPoint(Vector3 center, float range)
    {
        Vector3 result;
        Vector3 point = center + Random.insideUnitSphere * range;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(point, out hit, range, NavMesh.AllAreas))
        {
            result = hit.position;
        }
        else
        {
            result = new Vector3(center.x, center.y, center.z);
        }       

        return result;
    }

    private void HandleMosnterDeath(Monster monster)
    {
        monsterList.Remove(monster.gameObject);
    }
}
