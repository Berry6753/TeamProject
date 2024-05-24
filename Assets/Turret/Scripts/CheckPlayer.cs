using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayer : MonoBehaviour
{
    private Core core;

    private void Awake()
    {
        core = GameManager.Instance.GetCore.GetComponent<Core>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            core.isPlayer = true;
            core.player_Command.isCore = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            core.isPlayer = false;
            core.player_Command.isCore = false;
        }
    }
}
