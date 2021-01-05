using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    public class AmuletOfHealth : MonoBehaviour, IItemEffect
    {
        CharacterStats playerStats;
        public void Initialize(InitializeReason reason)
        {
            transform.SetParent(CharacterInventory.Instance.transform);

            Enemy.OnEnemyDie.AddListener(AddBuff);
            playerStats = Player.Instance.CharacterStats;

            gameObject.SetActive(false);
        }

        void AddBuff(Enemy enemy)
        {
            if (enemy.IsSummoned) return;

            playerStats.MaxHealth += 1;
        }
       
    }
}
