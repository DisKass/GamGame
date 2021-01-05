using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    public class Dice : MonoBehaviour, IItemEffect
    {
        public void Initialize(InitializeReason reason)
        {
            transform.SetParent(CharacterInventory.Instance.transform);
            //Player.Instance.CharacterStats.MaxHealth += 500;
            if (reason == InitializeReason.PICKUPPED)
            {
                Player.Instance.CharacterStats.CritChance += 0.25f;
            }
            gameObject.SetActive(false);
        }
    }
}