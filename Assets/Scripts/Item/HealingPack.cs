using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealingPack : MonoBehaviour
{
    [Header("전체 회복량")]
    [SerializeField]
    private float HealingValue;

    [Header("회복 지속 시간")]
    [SerializeField]
    private float HealingTime;

    [Header("회복 딜레이")]
    [SerializeField]
    private float HealingDelaySpeed;

    private GameObject player;

    private float healValue;
    private float totalTimer;
    private float timer;

    private void Awake()
    {
        healValue = (HealingValue / HealingTime);
    }

    private void OnEnable()
    {
        totalTimer = HealingTime;
        timer = 0;
        StartCoroutine(startHealing());
    }

    private void Update()
    {        
        if (player == null) return;
        Healing();
    }

    private IEnumerator startHealing()
    {
        while(totalTimer > 0)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            totalTimer -= Time.deltaTime;
        }
        transform.parent.gameObject.SetActive(false);
        yield break;
    }

    private void Healing()
    {
        timer += Time.deltaTime;
        if(timer >= HealingDelaySpeed)
        {
            //회복 효과
            player.GetComponent<Player_Info>().Heal(healValue * HealingDelaySpeed);
            //회복 딜레이 타이머 초기화
            timer = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;  
        }
    }

    private void OnDisable()
    {
        ItemObjectPool.ReturnToPool(transform.parent.gameObject);
    }
}
