using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Game.Items;

public class CharacterInventory : Singleton<CharacterInventory>
{
    [HideInInspector] public Transform _transform;

    protected override void Awake()
    {
        base.Awake();
        _transform = transform;
    }
    public void StoreItem(CollectableItem itemToStore)
    {
        ItemPools.Instance.RecieveItem(itemToStore);
        itemToStore.ItemDefinition.isResieved = true;
    }
}
