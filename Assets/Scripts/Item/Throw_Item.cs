using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Throw_Item : MonoBehaviour
{
    [Header("°ø°Ý ÀÌÆåÆ®")]
    [SerializeField]
    private GameObject skillEffect;

    [Header("»ý¼º ÁÂÇ¥ - YÁÂÇ¥")]
    [SerializeField]
    private float YPos;

    [Header("»ý¼º µô·¹ÀÌ")]
    [SerializeField]
    private float delayTimer;

    private Transform Player;

    private void Awake()
    {
        Player = GameObject.FindWithTag("Player").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Skill")) return;
        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            gameObject.SetActive(false);
            Invoke("Attack", delayTimer);            
        }
    }

    private void Attack()
    {
        ItemObjectPool.SpawnFromPool(skillEffect.name, new Vector3(transform.position.x, transform.position.y + YPos, transform.position.z), Quaternion.Euler(0, Player.eulerAngles.y, 0));
    }

    void OnDisable()
    {
        ItemObjectPool.ReturnToPool(gameObject);
    }
}
