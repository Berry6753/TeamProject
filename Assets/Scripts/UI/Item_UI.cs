using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Item_UI : MonoBehaviour
{
    public static Item_UI Instance;

    private void Awake()
    {
        Instance = this;
    }

    [Header("ItemIconImage")]
    [SerializeField]
    private Transform itemIconSlot;

    [Header("ItemCount")]
    [SerializeField]
    private TMP_Text itemCountText;

    private Image itemIcon;

    private void Start()
    {
        itemIcon = itemIconSlot.GetComponent<Image>();
        ChangeItemIcon();
        ItemCountNull();
    }

    public void ChangeItemIcon(Sprite image)
    {
        itemIcon.sprite = image;
        itemIcon.color = new Color(255, 255, 255, 255);
    }

    public void ChangeItemIcon()
    {
        itemIcon.sprite = null;
        itemIcon.color = new Color(0,0, 0,0);
    }

    public void ChangeItemCount(int count)
    {
        itemCountText.text = $"{count}";
    }

    public void ItemCountNull()
    {
        itemCountText.text = " ";
    }
}
