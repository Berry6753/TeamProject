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

    private Transform Player;

    private void Awake()
    {
        Player = GameObject.FindWithTag("Player").transform;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            gameObject.SetActive(false);
            ItemObjectPool.SpawnFromPool(skillEffect.name, new Vector3(transform.position.x, YPos, transform.position.z), Quaternion.Euler(0, Player.eulerAngles.y,0));           
        }
    }

    void OnDisable()
    {
        ItemObjectPool.ReturnToPool(gameObject);
    }
}
