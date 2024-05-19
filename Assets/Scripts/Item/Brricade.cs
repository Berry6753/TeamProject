using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brricade : MonoBehaviour
{
    [Header("바리게이트 체력")]
    [SerializeField]
    private int HP;

    [Header("밀려나는 힘")]
    [SerializeField]
    private float pushForce;

    private List<GameObject> target;

    private void Awake()
    {
        target = new List<GameObject>();
    }

    public void Hurt(float damage)
    {
        HP -= (int)damage;

        if(HP <= 0)
        {
            Destroyed();
        }
    }

    public void Destroyed()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        ItemObjectPool.ReturnToPool(gameObject);
    }

    private void FixedUpdate()
    {
        foreach(GameObject obj in target)
        {
            Vector3 dir = (obj.transform.position - transform.position);

            if (dir.z < 0)
            {
                obj.transform.position += new Vector3(0, 0, -0.8f);
            }
            else
            {
                obj.transform.position += new Vector3(0, 0, 0.8f);
            }

            //obj.transform.position = obj.transform.position + new Vector3(dir.x, 0, dir.z) * 1000f * Time.deltaTime;
            if (obj.CompareTag("Player"))
            {
                obj.GetComponent<CharacterController>().Move(new Vector3(dir.x, 0, dir.z) * pushForce * Time.deltaTime);
            }
            else if (obj.CompareTag("Monster"))
            {
                obj.GetComponent<Rigidbody>().AddForce(new Vector3(dir.x, 0, dir.z) * pushForce * Time.deltaTime);
            }
        }        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Skill")) return;
        target.Add(collision.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Skill")) return;
        target.Remove(collision.gameObject);
    }
}
