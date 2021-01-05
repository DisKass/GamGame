using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    public class Explosions : MonoBehaviour, IItemEffect
    {
        [SerializeField] Explore ExplosionPrefab;
        ObjectsPool explosions;
        Player player;
        public void Initialize(InitializeReason reason)
        {
            transform.SetParent(CharacterInventory.Instance.transform);
            player = Player.Instance;
            //Player.Instance.CharacterStats.MaxHealth += 500;
            WeaponManager.Instance.bulletInfo.OnBulletDestroyed += Explosion;
            explosions = new ObjectsPool(ExplosionPrefab.gameObject);
            gameObject.SetActive(false);
        }

        void Explosion(Bullet.BulletInfo bulletInfo, Vector2 position)
        {
            GameObject explosion = explosions.GetElement();
            explosion.transform.position = position;
            explosion.GetComponent<Explore>().damageSource = player.gameObject;
            explosion.GetComponent<Explore>().Activate();
        }
    }
}
