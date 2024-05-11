using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    [Header("LookAt")]
    [SerializeField]
    private Transform LookAt;

    private void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, LookAt.rotation, 15f * Time.deltaTime);
    }
}
