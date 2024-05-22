using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core_ZoomIn : MonoBehaviour
{
    private Player_Command player_command;
    private SkinnedMeshRenderer player_Mesh;
    private Animator animator;

    private CinemachineVirtualCamera cam;

    private readonly int hashZoom = Animator.StringToHash("Zoom");

    private void Awake()
    {
        cam = GetComponentInChildren<CinemachineVirtualCamera>();
        animator = GetComponent<Animator>();
        player_command = GameObject.FindAnyObjectByType<Player_Command>();
        player_Mesh = player_command.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
    }

    private void Update()
    {
        if (player_command.isCommand)
        {
            player_Mesh.enabled = false;
            cam.enabled = true;
            cam.Priority = 12;
            animator.SetBool(hashZoom, true);
        }
        else
        {
            animator.SetBool(hashZoom, false);
        }
    }

    public void ZoomOutEnd()
    {
        player_Mesh.enabled = true;
        cam.enabled = false;
        cam.Priority = 9;
    }
}
