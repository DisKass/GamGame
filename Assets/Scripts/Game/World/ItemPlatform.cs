using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    public class ItemPlatform : MonoBehaviour
    {
        [SerializeField] GameObject itemContainer;
        [SerializeField] GameObject chosenItem;
        Item currentItem;
        void Start()
        {
            if (chosenItem == null)
                chosenItem = ItemPools.Instance.GetRandomItemToInstantiate();
            //else chosenItem = ItemPool.Instance.GetItemToInstantiate(chosenItem.GetComponent<Item>());
            if (chosenItem == null)
            {
                Destroy(gameObject);
                return;
            }
            currentItem = Instantiate(chosenItem, itemContainer.transform).GetComponent<Item>();
            currentItem.Initialize();
            currentItem.transform.localPosition = Vector3.zero;
        }

        public int SetItem(GameObject item)
        {
            chosenItem = item;
            if (item == null)
            {
                Destroy(gameObject);
                return -1;
            }
            return item.GetComponent<Item>().ItemDefinition.ItemInfo.ID;
        }
        public void SetItem(int ID)
        {
            chosenItem = ItemPools.Instance.GetItemByID(ID);
            if (chosenItem == null)
            {
                Debug.Log("[ItemPlatform] Destroy");
                Destroy(gameObject);
            }
        }
        public GameObject GetItem()
        {
            return chosenItem;
        }
        private void OnDestroy()
        {
            if (currentItem != null)
            {
                if (ItemPools.Instance != null)
                    ItemPools.Instance.ReturnItem(chosenItem.GetComponent<Item>());
            }
        }
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject != Player.Instance.gameObject)
            {
                return;
            }
            if (collider.isTrigger == true) return;
            if (currentItem is CollectableItem item)
                item.StoreItemInInventory(InitializeReason.PICKUPPED);
            currentItem = null;
            Destroy(gameObject);

        }
    }
}