using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    public class DoubleAxe : MonoBehaviour, IItemEffect
    {
        [SerializeField] Bullet.DamageInfo damageInfo;
        Transform _transform;
        Transform playerTransform;
        private void Awake()
        {
            _transform = transform;
        }
        public void Initialize(InitializeReason reason)
        {
            damageInfo = WeaponManager.Instance.bulletInfo.DamageInfo;

            playerTransform = CharacterInventory.Instance.transform;
            _transform = new GameObject().transform;
            _transform.SetParent(playerTransform);
            _transform.localPosition = Vector3.right * 3;
            transform.SetParent(_transform);
            transform.localPosition = Vector3.down*0.89f; // 0.89f - from bottom of sprite to center
            GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            GetComponent<SpriteRenderer>().sortingOrder = 0;

            WeaponManager.Instance.OnDamageInfoChanged.AddListener(HandleDamageInfoChanged);
            GetComponent<Collider2D>().enabled = true;
            StartCoroutine(Rotate());
        }

        void HandleDamageInfoChanged(Bullet.DamageInfo damageInfo)
        {
            this.damageInfo = damageInfo;
        }

        IEnumerator Rotate()
        {
            while (true)
            {
                _transform.Rotate(new Vector3(0, 0, 360*Time.deltaTime));
                _transform.RotateAround(playerTransform.position, playerTransform.forward, 360 * Time.deltaTime);

                yield return new WaitForFixedUpdate();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out TakeDamage td))
            {
                if (other.isTrigger == true)
                {
                    td.Damage(damageInfo, playerTransform.gameObject);
                }
            }
        }
    }
}