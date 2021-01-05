using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game.Items;
using System;

public class ItemMenu : MonoBehaviour
{
    [SerializeField] GameObject itemContainer;
    [SerializeField] Button ResumeButton;
    [SerializeField] Button RestartButton;
    [SerializeField] Button MainMenuButton;
    [SerializeField] Button QuitButton;
    Sprite fieldImage;
    int count = 0;
    private List<int> recievedItems;
    public void Initialize()
    {
        fieldImage = itemContainer.transform.GetChild(0).GetComponent<Image>().sprite;
        ResumeButton.onClick.AddListener(HandleResumeClick);
        RestartButton.onClick.AddListener(HandleRestartClick);
        MainMenuButton.onClick.AddListener(HandleMainMenuClick);
        QuitButton.onClick.AddListener(HandleQuitClick);
        GameManager.Instance.OnStoreData.AddListener(HandleStoreData);
    }

    private void HandleStoreData(GameManager.StoreDataType type, string source)
    {
        if (type == GameManager.StoreDataType.RESET)
        {
            //Debug.Log("[ItemMenu] Reset.");
            for (int i = 0; i < count; i++)
            {
                itemContainer.transform.GetChild(i).GetComponent<ItemFrame>().RemoveItem();
            }
        }
    }

    private void OnEnable()
    {
        if (ItemPools.Instance == null) return;
        recievedItems = ItemPools.Instance.recievedItemsID;

        if (CharacterInventory.Instance != null) {
            while (recievedItems.Count > itemContainer.transform.childCount)
            {
                AddFields();
            }

            for (int i = 0; i < recievedItems.Count; i++) {
                itemContainer.transform.GetChild(i).GetComponent<ItemFrame>().SetItem(ItemPools.Instance.GetItemInfo(recievedItems[i]));
            }
            count = recievedItems.Count;
        }
    }
    
    void AddFields()
    {
        for (int i = 0; i < itemContainer.GetComponent<GridLayoutGroup>().constraintCount; i++)
            ((Image)Instantiate(new GameObject(), itemContainer.transform).AddComponent(typeof(Image))).sprite = fieldImage;
        itemContainer.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 208f);
    }
    void HandleResumeClick()
    {
        GameManager.Instance.TogglePause();
    }
    void HandleRestartClick()
    {
        GameManager.Instance.RestartGame();
    }
    void HandleMainMenuClick()
    {
        GameManager.Instance.ReturnMainMemu();
    }
    void HandleQuitClick()
    {
        GameManager.Instance.QuitGame();
    }
}
