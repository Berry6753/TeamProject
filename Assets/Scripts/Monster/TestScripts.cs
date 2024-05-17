using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestScripts : MonoBehaviour
{
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, transform.forward, Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("¿Ö °¡¶ó¾ÉÀ½?" + collision.gameObject.name);
    }
}
