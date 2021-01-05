using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    public class VampiricClaws : MonoBehaviour, IItemEffect
    {
        Player player;
        public void Initialize(InitializeReason reason)
        {
            player = Player.Instance;
            transform.SetParent(CharacterInventory.Instance.transform);
            //Player.Instance.CharacterStats.MaxHealth += 500;
            TakeDamage.OnAnyCharacterDamageRecieved.AddListener(HealPlayer);
            gameObject.SetActive(false);
        }

        void HealPlayer(Bullet.DamageInfo damageInfo, Transform targetTransform, GameObject source)
        {
            if (targetTransform != player.transform) player.Heal(Mathf.RoundToInt(damageInfo.Damage/10f));
        }
        private void OnDestroy()
        {
            TakeDamage.OnAnyCharacterDamageRecieved.RemoveListener(HealPlayer);
        }
    }
}
