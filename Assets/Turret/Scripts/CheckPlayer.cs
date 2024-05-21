using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Core.instance.isPlayer = true;
            Core.instance.player_Command.isCore = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Core.instance.isPlayer = false;
            Core.instance.player_Command.isCore = false;
        }
    }
}
