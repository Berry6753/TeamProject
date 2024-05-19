using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : MonoBehaviour
{
    [Header("걷는 소리 모음")]
    [SerializeField]
    private List<AudioClip> walkAudio;

    private AudioSource walkAudioSource;

    private void Awake()
    {
        walkAudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player")) return;
        //if (Mathf.Abs(Vector3.Dot(other.ClosestPoint(transform.position), Vector3.up)) <= 0.5f) return;
        if (other.transform.CompareTag("Desert"))
        {
            walkAudioSource.clip = walkAudio[0];
        }
        else if (other.transform.CompareTag("StoneRoad"))
        {
            walkAudioSource.clip = walkAudio[1];
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Turret"))
        {
            walkAudioSource.clip = walkAudio[2];
        }

        walkAudioSource.Play();
    }

    public void WalkSoundPlay()
    {
        //walkAudioSource.Play();
    }
}
