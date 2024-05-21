using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    //재생 시간
    private float timer;
    private void Awake()
    {
        CapsuleCollider = GetComponent<CapsuleCollider>();        
    }

    private void OnEnable()
    {
        targets.Clear();
        CapsuleCollider.radius = 0;
        CapsuleCollider.enabled = true;

        StartCoroutine(AugmentAttackArea());
        StartCoroutine(PlayAnimation());
        StartCoroutine(Attack());
    }
    
    IEnumerator Attack()
    {
        while (CapsuleCollider.enabled)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            foreach (var target in targets)
            {
                Debug.Log($"{target.name}에게 {AttackDamage}만큼의 데미지 부여");
            }
        }
        yield break;
    }

    IEnumerator AugmentAttackArea()
    {
        while(CapsuleCollider.radius < AttackArea)
        {
            yield return new WaitForSeconds(Time.deltaTime/3);
            CapsuleCollider.radius += 1.5f * Time.deltaTime;            
        }
        CapsuleCollider.enabled = false;
        targets.Clear();
        yield break;
    }

    IEnumerator PlayAnimation()
    {
        while (timer < 30)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            timer += Time.deltaTime;
        }

        transform.parent.gameObject.SetActive(false);
        yield break;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, CapsuleCollider.radius);
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Skill")) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Item")) return;
        if (!targets.Contains(other.gameObject))
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

    void OnDisable()
    {
        ItemObjectPool.ReturnToPool(transform.parent.gameObject);
    }
}
