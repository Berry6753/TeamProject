using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    public AudioSource[] musicSource;
    [HideInInspector] public float volume;

    public void SetMusicVolume(float value)
    {
        for (int i = 0; i < musicSource.Length; i++)
        {
            musicSource[i].volume = value;
        }
        volume = value;
    }
}
