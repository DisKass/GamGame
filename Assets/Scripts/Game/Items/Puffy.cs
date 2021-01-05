using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    [RequireComponent(typeof(PetAttack))]
    [RequireComponent(typeof(PetMove))]
    public class Puffy : MonoBehaviour, IItemEffect
    {
        PetAttack petAttack;
        PetMove petMove;
        public void Initialize(InitializeReason reason)
        {
            Portal.OnMovePlayer.AddListener(HandleMovePlayer);
            transform.SetParent(null);
            GameManager.Instance.MoveToScene(gameObject, "PlayerObjects");
            transform.position = Player.Instance.characterInventory._transform.position;
            petAttack = GetComponent<PetAttack>();
            petMove = GetComponent<PetMove>();
            petAttack.Initialize();
            petMove.Initialize();
        }

        void HandleMovePlayer(bool isStarted)
        {
            if (!isStarted)
            {
                transform.position = Player.Instance.characterInventory._transform.position;
            }
        }
    }
}
