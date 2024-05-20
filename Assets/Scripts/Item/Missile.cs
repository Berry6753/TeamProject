using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{

    [Header("공격 이펙트")]
    [SerializeField]
    private GameObject skillEffect;

    [Header("중력 가속도")]
    [SerializeField]
    private float GravityAddForce;

    private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(rigidbody.velocity.y < 0f)
        {
            rigidbody.AddForce(Vector3.down *  GravityAddForce, ForceMode.VelocityChange);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //이펙트 생성
        ItemObjectPool.SpawnFromPool(skillEffect.name, transform.position, transform.rotation);

        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        ItemObjectPool.ReturnToPool(gameObject);
    }
}
