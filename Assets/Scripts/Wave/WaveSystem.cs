using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField] private Wave[] waves;               //현재 웨이브 정보
    [SerializeField] private MonsterSpawner monsterSpawner;
    private int currentWaveIndex = 0;

    public void StartWave()
    {
        if (monsterSpawner.MonsterList.Count == 0 && currentWaveIndex < waves.Length)
        {
            currentWaveIndex++;
            monsterSpawner.StartWave(waves[currentWaveIndex]);
        }
    }
}

[System.Serializable]
public struct Wave
{
    public float spawnTime;             //현재 웨이브 적 생성 주기
    public int[] maxMonsterCount;           //현재 웨이브 적 등장 숫자
    public GameObject[] mosnterPrefab;    //몬스터 프리팹

}