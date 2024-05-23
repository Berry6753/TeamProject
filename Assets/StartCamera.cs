using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCamera : MonoBehaviour
{
    private Transform player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
    }
    private void OnEnable()
    {
        transform.position = player.position - player.forward * 3.5f;
    }
}
