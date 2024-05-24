using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingResolution : MonoBehaviour
{
    private TMP_Dropdown own;
    public TMP_Dropdown other;
    private void Awake()
    {
        own = GetComponent<TMP_Dropdown>();
    }

    private void OnEnable()
    {
        if (own.options.Count == other.options.Count) own.value = other.value;
    }
    //private void Update()
    //{
    //    Setting();
    //}

    //private void Setting()
    //{
    //    if (own.options.Count == other.options.Count) other.value = own.value;
    //}
}
