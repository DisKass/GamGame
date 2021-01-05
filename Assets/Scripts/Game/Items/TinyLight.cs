using System;
using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    public class TinyLight : MonoBehaviour, IItemEffect
    {
        GameObject player;
        Bullet.DamageInfo damageInfo;
        public void Initialize(InitializeReason reason)
        {
            player = Player.Instance.gameObject;

            transform.SetParent(CharacterInventory.Instance.transform);

            damageInfo = WeaponManager.Instance.bulletInfo.DamageInfo;
            damageInfo.Damage /= 2;
            WeaponManager.Instance.OnDamageInfoChanged.AddListener(HandleDamageInfoChanged);


            Enemy.OnEnemySpawned.AddListener(DamageEnemy);
            gameObject.SetActive(false);
        }

        private void HandleDamageInfoChanged(Bullet.DamageInfo damageInfo)
        {
            this.damageInfo = damageInfo;
            this.damageInfo.Damage /= 2;
        }

        private void DamageEnemy(Enemy enemy)
        {
            enemy.TakeDamage.Damage(damageInfo, player);
        }
    }
}
