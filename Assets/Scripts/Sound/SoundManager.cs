using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource[] musicSource;

    public void SetMusicVolume(float value)
    {
        for (int i = 0; i < musicSource.Length; i++)
        {
            musicSource[i].volume = value;
        }
    }
}
