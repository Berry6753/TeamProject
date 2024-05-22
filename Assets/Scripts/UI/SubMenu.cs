using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SubMenu : MonoBehaviour
{
    public Button firstFocus;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(firstFocus.gameObject);
    }

    public void Focus(Button button)
    {
        EventSystem.current.SetSelectedGameObject(button.gameObject);
    }
}
