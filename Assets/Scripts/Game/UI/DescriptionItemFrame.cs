using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game.Items;

public class DescriptionItemFrame : MonoBehaviour
{
    [SerializeField] Image ItemImage;
    [SerializeField] Text itemNameText;
    [SerializeField] Text descriptionText;
    public void ShowDescription(ItemInfo itemInfo)
    {
        ItemImage.enabled = true;
        ItemImage.sprite = itemInfo.Sprite;
        itemNameText.text = itemInfo.ItemName;
        descriptionText.text = itemInfo.Description;
    }
    private void OnDisable()
    {
        ItemImage.enabled = false;
        itemNameText.text = "";
        descriptionText.text = "";
    }
}
