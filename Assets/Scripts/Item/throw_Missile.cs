using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class throw_Missile : MonoBehaviour
{
    [Header("°ø°Ý ÀÌÆåÆ®")]
    [SerializeField]
    private GameObject skillEffect;

    [Header("ºÎ‹HÈú LayerMask")]
    [SerializeField]
    private LayerMask layerMask;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            gameObject.SetActive(false);
            ItemObjectPool.SpawnFromPool(skillEffect.name, new Vector3(transform.position.x, 50, transform.position.z));           
        }
    }

    void OnDisable()
    {
        ItemObjectPool.ReturnToPool(gameObject);
    }
}
