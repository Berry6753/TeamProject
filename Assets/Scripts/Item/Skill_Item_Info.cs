using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Attack,
    Heal,
    Defence
}

public class Skill_Item_Info : MonoBehaviour
{
    [Header("아이템 이름")]
    [SerializeField]
    private string itemName;

    [Header("아이템 타입")]
    [SerializeField]
    private ItemType type;

    [Header("아이템 Icon")]
    [SerializeField]
    private Sprite itemIcon;

    [Header("Effect Prefab")]
    [SerializeField]
    private GameObject effectPrefab;

    public Rigidbody body;

    public ItemType getType { get { return type; } }
    public string GetName { get { return itemName; } }
    public GameObject EffectPrefab { get {  return effectPrefab; } }
    public Sprite ItemIcon { get { return itemIcon; } }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.layer == layerMask)
    //    {
    //        ItemObjectPool.SpawnFromPool(effectPrefab.name, new Vector3(transform.position.x, 50, transform.position.z));
    //    }
    //}
    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        body.useGravity = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        body.useGravity=false;
        body.velocity = Vector3.zero;
    }

    private void OnDisable()
    {
        ItemObjectPool.ReturnToPool(gameObject);
    }
}
