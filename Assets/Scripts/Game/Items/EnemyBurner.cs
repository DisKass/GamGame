using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    public class EnemyBurner : MonoBehaviour, IItemEffect
    {
        public void Initialize(InitializeReason reason)
        {
            //Bullet.OnDamageDone.AddListener(HandleDamageDone);
            transform.SetParent(CharacterInventory.Instance.transform);
            if (reason == InitializeReason.PICKUPPED)
                Player.Instance.ChangeDamageType(Bullet.DamageTypeValues.ISFIRE, true);
            gameObject.SetActive(false);
        }
        //void HandleDamageDone(Bullet.BulletInfo bullet, GameObject target)
        //{
        //    if (target.activeInHierarchy)
        //        if (bullet.sourceTransform.tag == "Player")
        //        {
        //            target.GetComponent<Buff_Debuff_System>().Burn(bullet.DamageInfo.Damage / 10);
        //        }
        //}
    }
}
