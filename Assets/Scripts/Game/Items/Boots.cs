using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    public class Boots: MonoBehaviour, IItemEffect
    {
        public void Initialize(InitializeReason reason)
        {
            transform.SetParent(CharacterInventory.Instance.transform);
            if (reason == InitializeReason.PICKUPPED)
                Player.Instance.CharacterStats.SpeedMultiplier *= 1.2f;
            gameObject.SetActive(false);
        }
    }
}
