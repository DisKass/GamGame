using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    public enum ItemType { ITEM, WEAPON, EMPTY }

    [CreateAssetMenu(fileName = "New Item", menuName = "Item", order = 1)]
    public class Item_SO : ScriptableObject
    {
        [SerializeField] public ItemInfo ItemInfo;
        public bool isResieved;
    }

    [System.Serializable]
    public struct ItemInfo
    {
        public int ID;
        public string ItemName;
        public ItemType ItemType;
        public Sprite Sprite;
        [HideInInspector] public string Description;
    }
}