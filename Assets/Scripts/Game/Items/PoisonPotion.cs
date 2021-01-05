using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    public class PoisonPotion : MonoBehaviour, IItemEffect
    {
        [SerializeField] PoisonCircle poisonCircle;
        public void Initialize(InitializeReason reason)
        {
            Destroy(GetComponent<SetSortOrderByY>());
            Destroy(GetComponent<Item>());
            Destroy(GetComponent<SpriteRenderer>());
            transform.SetParent(CharacterInventory.Instance.transform);
            //Player.Instance.CharacterStats.MaxHealth += 500;
            poisonCircle = Instantiate(poisonCircle, Player.Instance.characterInventory.transform).GetComponent<PoisonCircle>();
            poisonCircle.transform.localScale = Vector3.one * 3;
            poisonCircle.Initialize();
            gameObject.SetActive(false);
        }
    }
}
