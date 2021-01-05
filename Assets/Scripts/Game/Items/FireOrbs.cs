using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    public class FireOrbs : MonoBehaviour, IItemEffect
    {
        [SerializeField] Bullet.DamageInfo damageInfo;
        [SerializeField] GameObject fireball;
        FireOrb[] myOrbs = new FireOrb[3];
        Transform _transform;
        int currentOrb = 0;
        float reload = 2f;
        Vector3 ownRotation = Vector3.zero;
        Vector3 fireDirection;
        //private void Start()
        //{
        //    Initialize(InitializeReason.PICKUPPED);
        //}
        public void Initialize(InitializeReason reason)
        {
            Portal.OnMovePlayer.AddListener((isStarted) =>
            {
                for (int i = 0; i < myOrbs.Length; i++)
                    myOrbs[i].SetActiveParticles(!isStarted);
            });
            Destroy(GetComponent<SetSortOrderByY>());
            Destroy(GetComponent<SpriteRenderer>());
            Destroy(GetComponent<CollectableItem>());
            _transform = transform;
            _transform.SetParent(Player.Instance.characterInventory._transform);
            _transform.localPosition = Vector2.zero;
            int count = 0;
            damageInfo = WeaponManager.Instance.bulletInfo.DamageInfo;
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent(out myOrbs[count]))
                {
                    myOrbs[count].gameObject.SetActive(true);
                    count++;
                }
            }
            reload = Player.Instance.CharacterStats.Reload * count;

            WeaponManager.Instance.OnDamageInfoChanged.AddListener(HandleDamageInfoChanged);
            WeaponManager.Instance.OnPlayerFired.AddListener(Fire);
            Player.Instance.OnWeaponRotated.AddListener((rotation) =>
            {
                fireDirection = rotation;
                _transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, rotation));
                _transform.Rotate(ownRotation);
            });
        }
        IEnumerator Reload(FireOrb orb)
        {
            yield return new WaitWhile(() => orb.enabled);
            orb.Restore();

        }
        void Fire()
        {
            if (myOrbs[currentOrb].IsWait)
            {
                myOrbs[currentOrb].Fire(damageInfo, fireDirection);
                //StartCoroutine(Reload(myOrbs[currentOrb]));
                currentOrb++;
                if (currentOrb >= myOrbs.Length) currentOrb = 0;
                ownRotation.z = 360 / myOrbs.Length * currentOrb;
                _transform.Rotate(new Vector3(0, 0, 360 / myOrbs.Length));
            }
        }
        void HandleDamageInfoChanged(Bullet.DamageInfo damageInfo)
        {
            this.damageInfo = damageInfo;
        }

    }
}
