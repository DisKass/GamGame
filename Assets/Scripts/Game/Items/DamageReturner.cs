using System;
using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    public class DamageReturner : MonoBehaviour, IItemEffect
    {
        Player player;
        public void Initialize(InitializeReason reason)
        {
            player = Player.Instance;
            transform.SetParent(CharacterInventory.Instance.transform);
            //Player.Instance.CharacterStats.MaxHealth += 500;
            player.TakeDamage.OnCharacterDamageRecieved.AddListener(HandleCharacterDamageRecieved);
            gameObject.SetActive(false);
        }

        private void HandleCharacterDamageRecieved(Bullet.DamageInfo damageInfo, Transform target, GameObject source)
        {
            if (source == player.gameObject) return;
            if (source.TryGetComponent(out TakeDamage t))
            {
                t.Damage(damageInfo, player.gameObject);
            }
        }
    }
}
