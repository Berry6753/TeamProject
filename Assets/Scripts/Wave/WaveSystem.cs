using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    private static int maxWave = 30;
    private float breakTime = 120.0f;
    [SerializeField] private Wave[] waves = new Wave[maxWave];               //현재 웨이브 정보
    [SerializeField] private MonsterSpawner monsterSpawner;
    public int currentWaveIndex = 0;
    public GameObject[] monster = new GameObject[4];
    NormalMonster normal;
    AerialMonster aerial;
    SiegeMonster siege;
    SuicideMonster suicide;

    private void Awake()
    {    
        for (int i = 0; i < waves.Length; i++)
        {
            waves[i].spawnTime = 1;
            waves[i].maxMonsterCount = new int[4];
            waves[i].monsterPrefab = new GameObject[4];
        }
    }
    private void Start()
    {
        normal = monster[0].GetComponent<NormalMonster>();
        aerial = monster[1].GetComponent<AerialMonster>();
        siege = monster[2].GetComponent<SiegeMonster>();
        suicide = monster[3].GetComponent<SuicideMonster>();
        for (int i = 0; i < monster.Length; i++)
        {
            for (int k = 0; k < waves.Length; k++)
            {
                waves[k].monsterPrefab[i] = monster[i];
                waves[k].maxMonsterCount[0] = normal.startSpawnNum;
                waves[k].maxMonsterCount[1] = aerial.startSpawnNum;
                waves[k].maxMonsterCount[2] = siege.startSpawnNum;
                waves[k].maxMonsterCount[3] = suicide.startSpawnNum;
            }
        }
        StartWave();
    }

    public void StartWave()
    {
        if (monsterSpawner.MonsterList.Count == 0 && currentWaveIndex < waves.Length)
        {
            currentWaveIndex++;
            monsterSpawner.StartWave(waves[currentWaveIndex]);
            if (monsterSpawner.MonsterList.Count == 0)
            { 
                StartCoroutine(BreakWave(breakTime));
            }
        }
    }

    private IEnumerator BreakWave(float time)
    {
        yield return new WaitForSeconds(time);
    }
}

[System.Serializable]
public class Wave
{
    public float spawnTime;               //현재 웨이브 적 생성 주기
    public int[] maxMonsterCount;         //현재 웨이브 적 등장 숫자
    public GameObject[] monsterPrefab;    //몬스터 프리팹  
}