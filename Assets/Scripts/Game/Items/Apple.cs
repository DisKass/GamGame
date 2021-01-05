using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    public class Apple: MonoBehaviour, IItemEffect
    {
        public void Initialize(InitializeReason reason)
        {
            transform.SetParent(CharacterInventory.Instance.transform);
            //Player.Instance.CharacterStats.MaxHealth += 500;
            if (reason == InitializeReason.PICKUPPED)
                Player.Instance.CharacterStats.HealthRegeneration += 5;
            gameObject.SetActive(false);
        }
    }
}
