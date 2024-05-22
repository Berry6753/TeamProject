using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableTest : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("aaaa");
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("cccccc");   
    }
}
