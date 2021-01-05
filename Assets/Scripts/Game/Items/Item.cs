using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    public abstract class Item : MonoBehaviour, IItem
    {
        [SerializeField] abstract public Item_SO ItemDefinition { get; set; }
        abstract public ItemInfo ItemInfo { get; set; }

        public virtual void Initialize()
        {
            ItemDefinition.ItemInfo.Description = GameText.Instance.GetText("DescriptionOfItem_" + ItemDefinition.ItemInfo.ID);
            ItemInfo = ItemDefinition.ItemInfo;
        }

        public static implicit operator GameObject (Item item)
        {
            return item.GetGameObject();
        }
        abstract public GameObject GetGameObject();
    }
}
