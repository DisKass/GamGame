using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    public class MagicShield : MonoBehaviour, IItemEffect
    {
        [SerializeField] MagicShieldComponent shield;
        public void Initialize(InitializeReason reason)
        {
            Destroy(GetComponent<CollectableItem>());
            Destroy(GetComponent<SetSortOrderByY>());
            Destroy(GetComponent<SpriteRenderer>());
            transform.SetParent(CharacterInventory.Instance.transform);
            transform.localPosition = Vector3.zero;
            shield = Instantiate(shield, transform);
            shield.Initialize();
            shield.transform.localPosition = Vector3.up * 0.4f;
            shield.transform.localScale = new Vector2(2, 2);

            //Player.Instance.CharacterStats.MaxHealth += 500;
            Player.Instance.PlayerController.OnPlayerStartsFire.AddListener(HandlePlayerFired);
        }

        void HandlePlayerFired()
        {
            shield.Activate();
        }
    }
}
