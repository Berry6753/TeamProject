using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreEffect : MonoBehaviour
{
    private void Awake()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
    }
}
