using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingSound : MonoBehaviour
{
    private Slider own;
    public Slider other;
    private void Awake()
    {
        own = GetComponent<Slider>();
    }

    private void Update()
    {
        SliderSetting();
    }

    private void SliderSetting()
    {
        other.value = own.value;
    }
}
