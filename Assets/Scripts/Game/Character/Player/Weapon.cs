using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Weapon : MonoBehaviour, IItemEffect
    {
        public Sprite BulletSprite;
        public float reloadMultiplier = 1f;
        public Bullet.DamageType damageType;
        public float spread = 0;

        public List<GameObject> _bulletSpawnPoints;
        private GameObject BulletSpawnPointsContainer;

        public Vector3 Direction { get => _transform.right; }

        Transform _transform;
        public bool rotatable = true;

        private void Awake()
        {
            _transform = transform;
            if (!rotatable)
            {
                BulletSpawnPointsContainer = new GameObject("BulletSpawnPointsContainer");
                BulletSpawnPointsContainer.transform.SetParent(_transform);
                BulletSpawnPointsContainer.transform.localPosition = Vector3.zero;
                _transform = BulletSpawnPointsContainer.transform;
            }
            for (int i = 0; i < transform.childCount; i++)
            {
                _bulletSpawnPoints.Add(transform.GetChild(i).gameObject);
                _bulletSpawnPoints[i].transform.SetParent(_transform);
            }
        }

        public void AddGunpoint(Vector2 bulletSpawnPoint, Vector2 direction)
        {
            GameObject gameObject = new GameObject();
            gameObject.transform.SetParent(BulletSpawnPointsContainer.transform);
            gameObject.transform.localPosition = bulletSpawnPoint;
            gameObject.transform.right = direction;
            _bulletSpawnPoints.Add(gameObject);
        }
        public void AddGunpoint()
        {
            AddGunpoint(_bulletSpawnPoints[0].transform.localPosition, _bulletSpawnPoints[0].transform.right);
        }

        public void Deinitialize()
        {
            Player.Instance.CharacterStats.ReloadMultiplier /= reloadMultiplier;
            Player.Instance.SubstractDamageType(damageType);
        }
        public void SetRotation(Quaternion rotation)
        {
            _transform.rotation = rotation;
        }
        public void Initialize(InitializeReason reason)
        {
            if (reason == InitializeReason.PICKUPPED)
            {
                Player.Instance.CharacterStats.ReloadMultiplier *= reloadMultiplier;
                Player.Instance.AddDamageType(damageType);
            }
        }
    }
}
