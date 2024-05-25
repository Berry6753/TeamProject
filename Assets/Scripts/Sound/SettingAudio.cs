using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingAudio : MonoBehaviour
{
    private AudioSource ownAudio;

    private void Awake()
    {
        ownAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        ownAudio.volume = GameManager.Instance.Sound.GetComponent<SoundManager>().volume;
    }
}
