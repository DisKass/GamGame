using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    public class Yin_yang : MonoBehaviour, IItemEffect
    {
        [SerializeField] GameObject shadowExplode;
        ObjectsPool ExplodesPool;
        Bullet.DamageInfo damageInfo;
        float Reload = 1f;
        float lastExplode = 0f;
        public void Initialize(InitializeReason reason)
        {
            ExplodesPool = new ObjectsPool(shadowExplode);

            transform.SetParent(CharacterInventory.Instance.transform);
            gameObject.SetActive(false);
            enabled = true;


            damageInfo = WeaponManager.Instance.bulletInfo.DamageInfo;
            Bullet.OnDamageDone.AddListener(HandleDamageDone);
            WeaponManager.Instance.OnDamageInfoChanged.AddListener(HandleDamageInfoChanged);
        }

        private void HandleDamageInfoChanged(Bullet.DamageInfo damageInfo)
        {
            this.damageInfo = damageInfo;
        }

        private void HandleDamageDone(Bullet.BulletInfo bulletInfo, GameObject target)
        {
            if (bulletInfo.sourceTransform.tag != "Player") return;
            if (!ShadowExplode.CanExplode) return;
            GameObject explode = ExplodesPool.GetElement();
            explode.transform.position = target.transform.position;
            explode.GetComponent<ShadowExplode>().Attack(damageInfo, target);
        }
    }
}