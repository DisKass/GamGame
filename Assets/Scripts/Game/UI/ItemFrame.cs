using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game.Items;

public class ItemFrame : MonoBehaviour
{
    private bool hasItem = false;
    private GameObject storedItem;
    Image myImage;
    [SerializeField] DescriptionItemFrame descriptionItem;
    [HideInInspector] private ItemInfo ItemInfo { get; set; }

    public void SetItem(ItemInfo itemInfo)
    {
        if (hasItem)
        {
            if (itemInfo.Equals(ItemInfo)) return;
        }
        else
        {
            storedItem = new GameObject();
            storedItem.transform.SetParent(transform);
            storedItem.transform.localPosition = Vector3.zero;
            storedItem.transform.localScale = Vector3.one;
            myImage = (Image)storedItem.AddComponent(typeof(Image));
            myImage.rectTransform.sizeDelta = new Vector2(130f, 130f);
            myImage.preserveAspect = true;
            hasItem = true;
        }
        ItemInfo = itemInfo;
        myImage.sprite = ItemInfo.Sprite;
    }
    public void RemoveItem()
    {
        if (hasItem)
        {
            Destroy(transform.GetChild(0).gameObject);
            hasItem = false;
        }
    }
    public void ShowDescription()
    {
        if (transform.childCount > 0)
        {
            descriptionItem.ShowDescription(ItemInfo);
        }
    }

}
