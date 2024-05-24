using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartCanvas : MonoBehaviour, IPointerEnterHandler
{
    #region Start

    public Image pointImage;
    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    #endregion

    #region Record



    #endregion

    #region Option

    FullScreenMode screenMode;
    public Toggle fullScreenBtn;
    public TMP_Dropdown resolutionDropdown;
    List<Resolution> resolutions = new List<Resolution>();
    public int resolutionNum;
    void Awake()
    {
        InitUI();
    }

    private void InitUI()
    { 
        //for (int i = 0; i < Screen.resolutions.Length; i++)
        //{
        //    if (Screen.resolutions[i].refreshRate == 60)  //일정hz만 넣고 싶을 때
        //    {
        //        resolutions.Add(Screen.resolutions[i]);
        //    }
        //}
        resolutions.AddRange(Screen.resolutions);       //hz 상관없이 넣고 싶을 때
        resolutionDropdown.options.Clear();
        int optionNum = 0;
        foreach (Resolution item in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = item.width + "x" + item.height + " " + item.refreshRate + "hz";
            resolutionDropdown.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
            { 
                resolutionDropdown.value = optionNum;
            }
            optionNum++;
        }
        resolutionDropdown.RefreshShownValue();

        fullScreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    public void DropboxOptionChange(int value)
    {
        resolutionNum = value;
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
    }

    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
    }

    #endregion

    #region Exit



    #endregion
}
