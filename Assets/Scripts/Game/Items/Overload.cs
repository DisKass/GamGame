using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    public class Overload : MonoBehaviour, IItemEffect
    {
        bool Overloaded = false;
        Player player;
        public void Initialize(InitializeReason reason)
        {
            Destroy(GetComponent<SetSortOrderByY>());
            Destroy(GetComponent<Item>());
            Destroy(GetComponent<SpriteRenderer>());
            transform.SetParent(CharacterInventory.Instance.transform);
            player = Player.Instance;
            player.CharacterStats.OnCharacterPropertyChanged.AddListener(HandlePropertyChanged);
        }

        private void HandlePropertyChanged(CharacterStats.PropertyID property, object value)
        {
            if (property == CharacterStats.PropertyID.HEALTH)
            {
                if (!Overloaded && player.CharacterStats.Health <= player.CharacterStats.MaxHealth * 0.4f)
                {
                    StartCoroutine(ActivateOverload());
                }
                if (Overloaded && player.CharacterStats.Health >= player.CharacterStats.MaxHealth * 0.5f)
                {
                    Debug.Log("Refresh");
                    Overloaded = false;
                }
            }
        }
        IEnumerator ActivateOverload()
        {
            Overloaded = true;
            player.CharacterStats.SpeedMultiplier += 0.5f;
            player.Rigidbody2D.mass *= 10;
            player.Buffs.IsImmune = true;
            yield return new WaitForSeconds(1f);
            player.Buffs.IsImmune = false;
            yield return new WaitForSeconds(2f);
            player.Rigidbody2D.mass /= 10;
            player.CharacterStats.SpeedMultiplier -= 0.8f;
            yield return new WaitForSeconds(2f);
            player.CharacterStats.SpeedMultiplier += 0.3f;
        }
    }
}
