using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    public class Shield : MonoBehaviour, IItemEffect
    {
        [SerializeField] Sprite collectedShield;
        [SerializeField] Sprite shieldSpike;
        Transform _transform;
        Transform _playerTransform;
        Transform _parentTransform;
        float[] fireAngles = new float[3] {
            45, 0, -45
            };
        private void Awake()
        {
            _transform = transform;
        }
        public void Initialize(InitializeReason reason)
        {
            GetComponent<SpriteRenderer>().sprite = collectedShield;

            _playerTransform = Player.Instance.Transform;

            _parentTransform = new GameObject().transform;
            _parentTransform.SetParent(CharacterInventory.Instance._transform);
            _parentTransform.localPosition = Vector3.zero;
            _transform = transform;
            _transform.SetParent(_parentTransform);
            _transform.localPosition = Vector3.right * 2;
            GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            GetComponent<SpriteRenderer>().sortingOrder = 0;

            WeaponManager.Instance.OnWeaponRotated.AddListener((rotation) =>
            {
                _parentTransform.right = rotation;
                
            });

            GetComponent<Collider2D>().enabled = true;
            //StartCoroutine(Rotate());
        }

        IEnumerator Rotate()
        {
            while (true)
            {
                _transform.RotateAround(_playerTransform.position, _playerTransform.forward, -90 * Time.deltaTime);

                yield return new WaitForFixedUpdate();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Bullet bullet))
            {
                if (bullet._bulletInfo.sourceTransform == _playerTransform) return;
                Bullet.BulletInfo _bulletInfo = bullet._bulletInfo;
                GameObject source = _bulletInfo.sourceTransform.gameObject;

                _bulletInfo.Sprite = shieldSpike;
                _bulletInfo.sourceTransform = _playerTransform;
                _bulletInfo.StartPosition = _transform.position;
                for (int i = 0; i < 3; i++)
                {
                    _bulletInfo.Rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, _transform.right) + fireAngles[i]);
                    FireManager.Instance.Fire(_bulletInfo);
                }
                _bulletInfo.DamageInfo.Damage = Mathf.RoundToInt(_bulletInfo.DamageInfo.Damage / 2f);
                _playerTransform.GetComponent<TakeDamage>().Damage(_bulletInfo.DamageInfo, source);
            }
        }
    }
}
