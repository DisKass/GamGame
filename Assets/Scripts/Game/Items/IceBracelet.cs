using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    public class IceBracelet : MonoBehaviour, IItemEffect
    {
        [SerializeField] int healthToSparkSpawn;
        [SerializeField] Sprite spark;
        [SerializeField] GameObject freezingCircle;
        int lostHealth;
        Bullet.BulletInfo bulletInfo = new Bullet.BulletInfo();
        Player player;

        public void Initialize(InitializeReason reason)
        {
            transform.SetParent(CharacterInventory.Instance.transform);
            transform.localPosition = Vector3.zero;
            Destroy(GetComponent<SetSortOrderByY>());
            Destroy(GetComponent<Item>());
            Destroy(GetComponent<SpriteRenderer>());

            freezingCircle = Instantiate(freezingCircle, transform);
            freezingCircle.transform.localPosition = Vector3.zero;
            freezingCircle.transform.localScale = Vector3.one * 4;
            freezingCircle.SetActive(true);

            player = Player.Instance;

            Bullet.DamageInfo damageInfo = new Bullet.DamageInfo();
            damageInfo.UpdateInfo(Damage: 10, WeaponManager.Instance.bulletInfo.DamageInfo.damageType);
            damageInfo.damageType.IsIce = true;
            bulletInfo.UpdateInfo(sourceTransform: player.transform, Damage: damageInfo, Speed: player.CharacterStats.BulletSpeed,
                LifeTime: 0.5f, Sprite: spark);

            //Bullet.OnDamageDone.AddListener(HandleDamageDone);
            Player.Instance.TakeDamage.OnCharacterDamageRecieved.AddListener(HandleDamageDone);
            WeaponManager.Instance.OnDamageTypeChanged.AddListener((type, value) => { if (type != Bullet.DamageTypeValues.ISICE) bulletInfo.ChangeDamageType(type, value); });
        }

        void HandleDamageDone(Bullet.DamageInfo damageInfo, Transform target, GameObject source)
        {
            lostHealth += damageInfo.Damage;
            if (lostHealth >= healthToSparkSpawn)
            {
                lostHealth = 0;
                freezingCircle.GetComponent<FreezingCircle>().Activate(bulletInfo.DamageInfo, player.gameObject);
                //bulletInfo.StartPosition = player.transform.position;
                //for (int i = 0; i < directions.Length; i++)
                //{
                //    bulletInfo.Rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, directions[i]));
                //    bulletInfo.Direction = directions[i];
                //    FireManager.Instance.Fire(bulletInfo);
                //}
            }
        }
    }
}