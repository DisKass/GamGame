using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    public class BulletPenetration : MonoBehaviour, IItemEffect
    {
        public void Initialize(InitializeReason reason)
        {
            WeaponManager.Instance.bulletInfo.penetration = true;
            transform.SetParent(CharacterInventory.Instance.transform);
            gameObject.SetActive(false);
        }
    }
}
