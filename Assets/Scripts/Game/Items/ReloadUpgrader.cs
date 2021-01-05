using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    public class ReloadUpgrader : MonoBehaviour, IItemEffect
    {
        public void Initialize(InitializeReason reason)
        {
            transform.SetParent(CharacterInventory.Instance.transform);
            //Player.Instance.CharacterStats.MaxHealth += 500;
            if (reason == InitializeReason.PICKUPPED)
            {
                Player.Instance.CharacterStats.ReloadMultiplier *= 0.5f;
                Player.Instance.CharacterStats.DamageMultiplier *= 0.6f;
            }
            gameObject.SetActive(false);
        }
    }
}
