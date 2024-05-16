using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    [Header("회전 속도")]
    [SerializeField]
    private float rotationSpeed;

    private float GearCount;

    private void OnEnable()
    {
        int GearRandom = Random.Range(0, 100);
        if (GearRandom < 60) GearCount = 1f;
        else if (GearRandom < 80) GearCount = 2f;
        else if (GearRandom < 92) GearCount = 3f;
        else if (GearRandom < 97) GearCount = 4f;
        else GearCount = 5f;
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player_Info>().AddGearCount((int)GearCount);
            transform.gameObject.SetActive(false);
        }
    }

}
