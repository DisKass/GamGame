using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    public class CollectableItem : Item
    {
        [SerializeField] Item_SO _itemDefinition;
        [HideInInspector] public override Item_SO ItemDefinition { get => _itemDefinition; set => _itemDefinition = value; }
        [HideInInspector] public override ItemInfo ItemInfo { get; set; }
        
        public override GameObject GetGameObject()
        {
            return gameObject;
        }

        private IItemEffect itemScript;
        public override void Initialize()
        {
            base.Initialize();
            itemScript = gameObject.GetComponent<IItemEffect>();
        }
        public void StoreItemInInventory(InitializeReason reason)
        {
            itemScript.Initialize(reason);

            CharacterInventory.Instance.StoreItem(this);
        }

    }
}