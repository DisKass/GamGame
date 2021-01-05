using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    public class AmuletOfDamage : MonoBehaviour, IItemEffect
    {
        int currentBuff = 0;
        int enemyDiedCount = 0;
        CharacterStats playerStats;
        public void Initialize(InitializeReason reason)
        {
            transform.SetParent(CharacterInventory.Instance.transform);

            Enemy.OnEnemyDie.AddListener(AddBuff);
            LevelStateController.OnLevelStateChanged.AddListener(HandleLevelStateChanged);
            playerStats = Player.Instance.CharacterStats;

            gameObject.SetActive(false);
        }

        void AddBuff(Enemy enemy)
        {
            enemyDiedCount++;
            if (enemyDiedCount < 4) return;
            enemyDiedCount = 0;
            currentBuff += 1;
            playerStats.TemporaryDamage += 1;
        }
        void HandleLevelStateChanged(LevelStateController.LevelState levelState)
        {
            if (levelState == LevelStateController.LevelState.BEGINNING)
            {
                RemoveBuff();
            }
        }
        void RemoveBuff()
        {
            playerStats.TemporaryDamage -= currentBuff;
            currentBuff = 0;
            enemyDiedCount = 0;
        }
    }
}
