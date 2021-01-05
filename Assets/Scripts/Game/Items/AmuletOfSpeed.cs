using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    public class AmuletOfSpeed : MonoBehaviour, IItemEffect
    {
        float currentBuff = 0;
        int maxBuffs = 50;
        int buffsCount = 0;
        CharacterStats playerStats;
        public void Initialize(InitializeReason reason)
        {
            transform.SetParent(CharacterInventory.Instance.transform);

            Enemy.OnEnemyDie.AddListener(AddBuff);
            LevelStateController.OnLevelStateChanged.AddListener(HandleLevelStateChanged);
            playerStats = Player.Instance.CharacterStats;
            //Player.Instance.CharacterStats.MaxHealth += 500;
            gameObject.SetActive(false);
        }

        void AddBuff(Enemy enemy)
        {
            //currentBuff += playerStats.ReloadMultiplier * 0.05f;
            //if (buffsCount < maxBuffs)
            //{
            //    playerStats.ReloadMultiplier -= playerStats.ReloadMultiplier * 0.05f;
            //    buffsCount++;
            //}
            currentBuff -= 0.01f;
            playerStats.TemporaryReload -= 0.01f;
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
            playerStats.TemporaryReload -= currentBuff;
            currentBuff = 0;
        }
    }
}
