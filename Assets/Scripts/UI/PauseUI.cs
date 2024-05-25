using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject optionUI;

    private void OnEnable()
    {
        mainUI.SetActive(true);
        optionUI.SetActive(false);
    }
}
