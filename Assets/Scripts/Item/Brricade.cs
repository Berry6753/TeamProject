using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Brricade : MonoBehaviour
{
    [Header("바리게이트 체력")]
    [SerializeField]
    private int HP;

    [Header("밀려나는 힘")]
    [SerializeField]
    private float pushForce;

    private List<GameObject> target;
    private bool isGroound;
    private Rigidbody body;

    private NavMeshObstacle obstacle;
    
    private void Awake()
    {
        target = new List<GameObject>();
        body = GetComponent<Rigidbody>();
        obstacle = GetComponent<NavMeshObstacle>();
    }

    private void OnEnable()
    {
        isGroound = false;
        obstacle.enabled = false;
    }

    public void Hurt(int damage)
    {
        HP -= damage;

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
        isGroound = true;
        target.Clear();
        ItemObjectPool.ReturnToPool(gameObject);
    }

    private void FixedUpdate()
    {
        if (isGroound)
        {
            return;
        }
        foreach (GameObject obj in target)
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
        if(!isGroound)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")|| collision.gameObject.layer == LayerMask.NameToLayer("Turret"))
            {
                isGroound = true;
                body.constraints = RigidbodyConstraints.FreezeAll;
                obstacle.enabled = true;
                return;
            }
            if (collision.gameObject.layer == LayerMask.NameToLayer("Skill")) return;
            target.Add(collision.gameObject);
            
        }        
    }

    
}
