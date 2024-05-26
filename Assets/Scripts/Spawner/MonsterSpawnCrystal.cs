using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MonsterSpawnCrystal : MonoBehaviour
{
    [Header("Ã¼·Â")]
    [SerializeField]
    protected float HP;

    [Header("ÆÄ±« Effect")]
    [SerializeField]
    protected GameObject effect;

    protected float timer;

    protected MonsterSpawner monsterSpawner;

    protected virtual void Awake()
    {
        monsterSpawner = GameManager.Instance.MonsterSpawner;
        effect = transform.GetChild(1).gameObject;
    }

    protected virtual void OnEnable()
    {
        effect.SetActive(false);
        timer = 0f;
    }

    public virtual void Hurt(float damage)
    {
        HP -= damage;
        if(HP <= 0)
        {
            monsterSpawner.DestorySpawner(this.transform);
            effect.SetActive(true);
            effect.transform.parent = null;
            gameObject.SetActive(false);
        }        
    }

    protected virtual void Update()
    {
        if(effect.activeSelf)
        {
            timer += Time.deltaTime;
            if(timer >= 2)
            {
                effect.SetActive(false);
                timer = 0f;
            }
        }
    }

}
