using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    public class AncientKnife : MonoBehaviour, IItemEffect
    {
        public void Initialize(InitializeReason reason)
        {
            transform.SetParent(CharacterInventory.Instance.transform);
            if (reason == InitializeReason.PICKUPPED)
            {
                Player.Instance.CharacterStats.Damage = Player.Instance.CharacterStats.BaseDamage + 10;
            }
            gameObject.SetActive(false);
        }
    }
}
