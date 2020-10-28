using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [System.Serializable]
    public enum DamageTypeValues
    {
        ISCRITICAL,
        ISFIRE,
        ISICE,
        ISDOT
    }
    [System.Serializable]
    public struct DamageType
    {
        public const int typesCount = 4;
        public bool IsCritical;
        public bool IsFire;
        public bool IsIce;
        public bool IsDot;
        public void UpdateInfo(bool IsCritical = false, bool IsFire = false, bool IsIce = false,  bool IsDot = false)
        {
            this.IsCritical = IsCritical;
            this.IsFire = IsFire;
            this.IsDot = IsDot;
            this.IsIce = IsIce;
        }
        public void UpdateInfo(DamageType info)
        {
            IsCritical = info.IsCritical;
            IsFire = info.IsFire;
            IsDot = info.IsDot;
        }
        public DamageType(bool IsCritical = false, bool IsFire = false, bool IsIce = false, bool IsDot = false)
        {
            this.IsCritical = IsCritical;
            this.IsFire = IsFire;
            this.IsDot = IsDot;
            this.IsIce = IsIce;
        }
        public bool this[int index] {
            get
            {
                switch (index) {
                    case 0: return IsCritical;
                    case 1: return IsFire;
                    case 2: return IsIce;
                    case 3: return IsDot;
                    default: Debug.LogError("[DamageType] Out of range. Index: " + index); break;
                }
                return false;
            }
            set
            {
                switch (index)
                {
                    case 0: IsCritical = value; break;
                    case 1: IsFire = value; break;
                    case 2: IsIce = value; break;
                    case 3: IsDot = value; break;
                    default: Debug.LogError("[DamageType] Out of range. Index: " + index); break;
                }
            }
        }
        public static DamageType operator + (DamageType a, DamageType b)
        {
            for (int i = 0; i < typesCount; i++)
            {
                a[i] = a[i] || b[i];
            }
            return a;
        }         
        public static DamageType operator - (DamageType a, DamageType b)
        {
            for (int i = 0; i < typesCount; i++)
            {
                a[i] = a[i] && !b[i];
            }
            return a;
        }
    }
    [System.Serializable]
    public struct DamageInfo
    {
        public int Damage;
        public float CritChance { get; set; }
        public float CritMultiplier { get; set; }
        public DamageType damageType;
        public void UpdateInfo(int Damage = 0, DamageType damageType = default)
        {
            this.Damage = Damage;
            this.damageType = damageType;
        }
    }
    public struct BulletInfo
    {
        public DamageInfo DamageInfo;
        public Transform sourceTransform { get; set; }
        public float Speed { get; set; }
        public Vector2 Direction { get; set; }
        public Vector2 StartPosition { get; set; }
        public float LifeTime { get; set; }

        public bool penetration;
        public Sprite Sprite { get; set; }
        public Quaternion Rotation { get; set; }
        public Action<BulletInfo, Vector2> OnBulletDestroyed { get; set; }
        public void UpdateInfo(Transform sourceTransform = default, DamageInfo Damage = default, float Speed = 0,
            Vector2 Direction = default, Vector2 StartPosition = default, float LifeTime = 0,
            float CritChance = 0, float CritMultiplier = 0, bool penetration = false, Sprite Sprite = null, Quaternion Rotation = default)
        {
            this.sourceTransform = sourceTransform;
            this.DamageInfo = Damage;
            this.Speed = Speed;
            this.Direction = Direction;
            this.StartPosition = StartPosition;
            this.LifeTime = LifeTime;
            this.DamageInfo.CritChance = CritChance;
            this.DamageInfo.CritMultiplier = CritMultiplier;
            this.penetration = penetration;
            if (Sprite != null) this.Sprite = Sprite;
            this.Rotation = Rotation;
        }

        public void UpdateProperty(CharacterStats.PropertyID property, object value)
        {
            switch (property)
            {
                case CharacterStats.PropertyID.CRITCHANCE:
                    DamageInfo.CritChance = (float)value;
                    break;
                case CharacterStats.PropertyID.CRITMULTIPLIER:
                    DamageInfo.CritMultiplier = (float)value;
                    break;
                case CharacterStats.PropertyID.BULLETSPEED:
                    Speed = (float)value;
                    break;
                case CharacterStats.PropertyID.BULLETLIFETIME:
                    LifeTime = (float)value;
                    break;
                default:
                    break;
            }
        }

        public void ChangeDamageType(DamageTypeValues damageType, bool value)
        {
            DamageInfo.damageType[(int)damageType] = value;
        }
    }

    public BulletInfo _bulletInfo;
    [SerializeField] SpriteRenderer spriteRenderer;
    

    public static Events.EventDamageDone OnDamageDone = new Events.EventDamageDone();

    [SerializeField] Rigidbody2D _rigidbody;

    public Bullet UpdateValues(BulletInfo bulletInfo)
    {
        _bulletInfo = bulletInfo;
        if (spriteRenderer == null)
        {
            Debug.LogError("sprite Renderer is null");
        }
        if (_bulletInfo.Sprite != null)
            spriteRenderer.sprite = _bulletInfo.Sprite;
        transform.rotation = _bulletInfo.Rotation;
        return this;
    }


    public Bullet Fire()
    {
        transform.position = _bulletInfo.StartPosition;
        //rigidbody.transform.rotation = Quaternion.Euler(_bulletInfo.Direction);
        _rigidbody.velocity = _bulletInfo.Speed * transform.right;// _bulletInfo.Direction.normalized;
        StartCoroutine(moving());
        return this;
    }
    IEnumerator moving()
    {
        yield return new WaitForSeconds(_bulletInfo.LifeTime);
        BreakBullet();
    }

    //void UpdateDamageInfo()
    //{
    //    _damageInfo.Damage = _bulletInfo.Damage;

    //    _damageInfo.isCritical = Random.value < _bulletInfo.CritChance;
    //    if (_damageInfo.isCritical)
    //        _damageInfo.Damage = _bulletInfo.Damage * _bulletInfo.CritMultiplier;
    //}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_bulletInfo.sourceTransform == null)
        {
            Debug.Log("sourceTransform is null");
            return;
        }
        if (other.tag == _bulletInfo.sourceTransform.tag || other.GetComponent<Bullet>() != null)
            return;
        if (!other.isTrigger)
        {
            if (other.GetComponent<ICharacter>() != null)
                return;
        }
        if (other.TryGetComponent(out TakeDamage targetTakeDamage))
        {
            if (other.isTrigger == true)
            {
                targetTakeDamage.Damage(_bulletInfo.DamageInfo, _bulletInfo.sourceTransform.gameObject);
                OnDamageDone.Invoke(_bulletInfo, targetTakeDamage.gameObject);
                //td.DotDamage(_damageInfo);
            }
        }
        if (!_bulletInfo.penetration)
        {
            BreakBullet();
        }
    }

    void BreakBullet()
    {
        _bulletInfo.OnBulletDestroyed?.Invoke(_bulletInfo, transform.position);
        _bulletInfo.OnBulletDestroyed = null;
        DestructToParticles.Instance.Destruct(_bulletInfo.Sprite, transform.position);
        gameObject.SetActive(false);
    }
}
