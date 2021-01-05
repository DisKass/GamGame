using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    public class ItemPools : Singleton<ItemPools>
    {
        [SerializeField] ItemPool defaultItems;
        [HideInInspector] public List<int> recievedItemsID;
        private string PoolName;

        private void Start()
        {
            GameManager.Instance.OnStoreData.AddListener(HandleStoreData);
            recievedItemsID = Load();
            Debug.Log(recievedItemsID.Count);
            //Debug.Log("RecievedItemsIDs: " + recievedItemsID.Count);
            defaultItems.Initialize(recievedItemsID);
        }

        public ItemInfo GetItemInfo(int ID)
        {
            if (defaultItems.Contains(ID))
            {
                return defaultItems.GetItemInfo(ID);
            }
            else
            {
                throw new System.Exception("defaultItems Not cintains item with ID: " + ID);
            }
        }

        void HandleStoreData(GameManager.StoreDataType type, string source)
        {
            if (source == "ItemPool")
            {
                //Debug.Log("[ItemPool] type: " + type);
                if (type == GameManager.StoreDataType.SAVE) Save();
                if (type == GameManager.StoreDataType.LOAD) Load();
                //if (type == GameManager.StoreDataType.RESET) DeleteSaveFile();
            }
        }
        void Save()
        {
            SaveSystem<ItemPools, ItemPoolData>.Save(this, "ItemPool");
        }
        List<int> Load() {
            if (SaveSystem<ItemPools, ItemPoolData>.CheckSaveFile("ItemPool"))
            {
                //Debug.Log("[ItemPool] Loaded");
                return SaveSystem<ItemPools, ItemPoolData>.Load("ItemPool").RecievedItemsID;
            }
            else
            {
                //Debug.Log("[ItemPool] NotLoaded");
                return new List<int>();
            }
        }
        public GameObject GetRandomItemToInstantiate()
        {
            return defaultItems.GetRandomItemToInstantiate();
        }
        public GameObject GetItemByID(int ID)
        {
            return defaultItems.GetItemByID(ID);
        }
        public void ReturnItem(Item item)
        {
            if (defaultItems.Contains(item.ItemDefinition.ItemInfo.ID))
            {
                defaultItems.ReturnItem(item);
            }
        }

        public GameObject GetItemToInstantiate(Item item)
        {
            return defaultItems.GetItemToInstantiate(item);
        }
        public void RecieveItem(Item item)
        {
            if (defaultItems.Contains(item.ItemDefinition.ItemInfo.ID))
            {
                if (recievedItemsID.Contains(item.ItemDefinition.ItemInfo.ID) == false)
                    recievedItemsID.Add(item.ItemDefinition.ItemInfo.ID);
                defaultItems.RecieveItem(item);
            }
        }

    }

    [System.Serializable]
    public class ItemPool
    {
        [SerializeField] List<Item> allItems;
        [HideInInspector] public List<Item> recievedItems = new List<Item>();
        [HideInInspector] public List<Item> notRecievedItems = new List<Item>();
        [HideInInspector] private List<Item> instantiatedItems = new List<Item>();
        [HideInInspector] private List<Item> notInstantiatedItems = new List<Item>();

        public bool Contains(int ID)
        {
            foreach (Item i in allItems)
            {
                if (i.ItemDefinition.ItemInfo.ID == ID) return true;
            }
            return false;
        }
        public ItemInfo GetItemInfo(int ID)
        {
            foreach (Item i in allItems)
            {
                if (i.ItemDefinition.ItemInfo.ID == ID) return i.ItemDefinition.ItemInfo;
            }
            return default;
        }
        public void Initialize(List<int> recievedItemsID)
        {
            foreach (Item go in allItems)
            {
                notRecievedItems.Add(go);
                if (recievedItemsID.Contains(go.ItemDefinition.ItemInfo.ID))
                {
                    //Debug.Log(go.ItemDefinition.ItemInfo.ItemName + " stored");
                    Item item = GameObject.Instantiate(go).GetComponent<Item>();
                    item.Initialize();
                    if (item is CollectableItem colItem)
                        colItem.StoreItemInInventory(InitializeReason.LOADED);
                }
                else notInstantiatedItems.Add(go);
            }
        }

        public GameObject GetRandomItemToInstantiate()
        {
            if (notInstantiatedItems.Count == 0) return null;
            Item result = notInstantiatedItems[Random.Range(0, notInstantiatedItems.Count)];
            instantiatedItems.Add(result);
            notInstantiatedItems.Remove(result);
            return result;
        }
        public GameObject GetItemByID(int ID)
        {
            foreach (Item i in notInstantiatedItems)
            {
                if (i.ItemDefinition.ItemInfo.ID == ID)
                {
                    if (recievedItems.Contains(i)) return null;
                    instantiatedItems.Add(i);
                    notInstantiatedItems.Remove(i);
                    //Debug.Log("NotNull + " + recievedItems.Contains(i));
                    return i;
                }
            }
            //Debug.Log("Null");
            return null;
        }
        public void ReturnItem(Item item)
        {
            instantiatedItems.Remove(item);
            notInstantiatedItems.Add(item);
        }

        public GameObject GetItemToInstantiate(Item item)
        {
            foreach (Item i in notInstantiatedItems)
            {
                if (i == item)
                {
                    instantiatedItems.Add(i);
                    notInstantiatedItems.Remove(i);
                    return i;
                }
            }
            return null;
        }
        public void RecieveItem(Item item)
        {
            //Debug.Log("Added + " + item.ItemDefinition.ItemInfo.ID);
            notRecievedItems.Remove(item);
            recievedItems.Add(item);
            Debug.Log("Save");
            GameManager.Instance.SaveGame("ItemPool");
        }
    }
}
