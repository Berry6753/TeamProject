using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretIcon_UI : MonoBehaviour
{
    private List<string> turretScripts;

    private GameObject printTurret;

    public void PrintSelectTurret(int index)
    {
        if(turretScripts == null) turretScripts = MultiObjectPool.inst.GetPoolsTag();
        if (printTurret != null)
        {
           // printTurret.GetComponent<MeshRenderer>().receiveShadows = true;
            printTurret.SetActive(false);
        }
        printTurret = MultiObjectPool.SpawnFromPool(turretScripts[index], transform.position, Quaternion.Euler(0,-90,0));
        //printTurret.GetComponent<MeshRenderer>().receiveShadows = false;
    }
}
