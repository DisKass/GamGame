using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    public class VampiricHorns : MonoBehaviour, IItemEffect
    {
        Player player;
        public void Initialize(InitializeReason reason)
        {
            player = Player.Instance;
            transform.SetParent(CharacterInventory.Instance.transform);
            //Player.Instance.CharacterStats.MaxHealth += 500;
            if (reason == InitializeReason.PICKUPPED)
            {
                player.CharacterStats.MaxHealth += 50;
                player.CharacterStats.HealthRegenerationMultiplier += 0.4f;
            }
            gameObject.SetActive(false);
        }
    }
}
