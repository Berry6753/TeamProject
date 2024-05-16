using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    [Header("공격 데미지")]
    [SerializeField]
    private float AttackDamage;

    List<GameObject> targets = new List<GameObject>();

    private CapsuleCollider CapsuleCollider;

    [Header("최대 공격 범위")]
    [SerializeField]
    private float AttackArea;

    public float damage { get { return AttackDamage; } }

    private void Awake()
    {
        CapsuleCollider = GetComponent<CapsuleCollider>();        
    }

    private void OnEnable()
    {
        targets.Clear();
        CapsuleCollider.radius = 0;

        StartCoroutine(AugmentAttackArea());

        foreach (var target in targets)
        {
            Debug.Log($"{target.name}에게 {AttackDamage}만큼의 데미지 부여");
        }
    }

    IEnumerator AugmentAttackArea()
    {
        while(CapsuleCollider.radius < AttackArea)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            CapsuleCollider.radius += 1.5f * Time.deltaTime;            
        }
        
        yield return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, CapsuleCollider.radius);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.CompareTag("Untagged")) return;
        if(!targets.Contains(other.gameObject))
        {
            targets.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(targets.Contains(other.gameObject))
        {
            targets.Remove(other.gameObject);
        }
    }
}
