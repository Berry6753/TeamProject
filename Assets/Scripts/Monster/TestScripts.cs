using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestScripts : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] private float hp;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            Debug.Log("피해받음");
        }
    }

    private void Hurt(float damage)
    { 
        hp -= damage;
    }
}
