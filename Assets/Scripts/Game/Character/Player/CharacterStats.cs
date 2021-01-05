using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public enum PropertyID : uint
    {
        MAXHEALTH,
        MAXHEALTHMULTIPLIER,
        HEALTH,
        HEALTHREGENERATION,
        DAMAGE,
        DAMAGEMULTIPLIER,
        CRITCHANCE,
        CRITMULTIPLIER,
        SPEED,
        SPEEDMULTIPLIER,
        DRAG,
        RELOAD,
        RELOADMULTIPLIER,
        BULLETSPEED,
        BULLETLIFETIME,
        CURRENTWEAPON,
        ATTACKRANGE,
        SPRITE,
        ANIMATORCONTROLLER,
        POSITION,
        DEADNESS
    }
    [SerializeField] public CharacterStats_SO _characterStats;

    public Events.EventCharacterPropertyChanged OnCharacterPropertyChanged;
    #region Fields

    [HideInInspector] public int level = 2;
    
    [System.Serializable]
    public struct SerializableStats
    {
        public int maxHealth;
        public int health;
        public int healthRegeneration;
        public int damage;
        public float critChance;
        public float critMultiplier;
        public float speed;
        public float drag;
        public float reload;
        public float bulletSpeed;
        public float bulletLifeTime;
        public float attackRange;

        public SerializableMultiplierStats multipliers;
        [System.NonSerialized]
        public TemporaryStats temporaryStats;
        public int currentWeapon;
        public bool isDead;
    }
    [System.Serializable]
    public struct SerializableMultiplierStats
    {
        public float maxHealth;
        public float regeneration;
        public float damage;
        public float speed;
        public float reload;
    }
    public struct TemporaryStats
    {
        public int damage;
        public float reload;
    }

    SerializableStats stats = new SerializableStats();

    private Sprite sprite;
    private RuntimeAnimatorController playerAnimatorController;

    private Vector2 position = Vector2.zero;
    

    #endregion
    private void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener((a, b) =>
        {
            if (a == GameManager.GameState.PREGAME_RUNNING_SWITCH && b == GameManager.GameState.RUNNING) Initialize();
        });
    }
    public SerializableStats GetSerializableStats()
    {
        return stats;
    }
    public void Initialize()
    {
        level = _characterStats.level;
        MaxHealth = _characterStats.maxHealth;
        MaxHealthMultiplier = _characterStats.maxHealthMultiplier;
        Health = _characterStats.maxHealth;
        HealthRegeneration = _characterStats.healthRegeneration;
        HealthRegenerationMultiplier = _characterStats.healthRegenerationMultiplier;
        Damage = _characterStats.damage;
        DamageMultiplier = _characterStats.damageMultiplier;
        CritChance = _characterStats.critChance;
        CritMultiplier = _characterStats.critMultiplier;
        Speed = _characterStats.speed;
        SpeedMultiplier = _characterStats.speedMultiplier;
        Drag = _characterStats.drag;
        Reload = _characterStats.reload;
        ReloadMultiplier = _characterStats.reloadMultiplier;
        BulletSpeed = _characterStats.bulletSpeed;
        BulletLifeTime = _characterStats.bulletLifeTime;
        AttackRange = _characterStats.attackRange;
        CurrentWeapon = _characterStats.currentWeapon;
        Sprite = _characterStats.sprite;
        PlayerAnimatorController = _characterStats._playerAnimatorController;
        IsDead = _characterStats.IsDead;
        //Debug.Log("[CharacterStats] Level: " + level);
    }

    public void SetLoadedData(SerializableStats serializableStats)
    {
        MaxHealth = serializableStats.maxHealth;
        MaxHealthMultiplier = serializableStats.multipliers.maxHealth;
        Health = serializableStats.health;
        HealthRegeneration = serializableStats.healthRegeneration;

        HealthRegenerationMultiplier = serializableStats.multipliers.regeneration;
        Damage = serializableStats.damage;
        DamageMultiplier = serializableStats.multipliers.damage;
        CritChance = serializableStats.critChance;
        CritMultiplier = serializableStats.critMultiplier;
        Speed = serializableStats.speed;
        SpeedMultiplier = serializableStats.multipliers.speed;
        Drag = serializableStats.drag;
        Reload = serializableStats.reload;
        ReloadMultiplier = serializableStats.multipliers.reload;
        BulletSpeed = serializableStats.bulletSpeed;
        BulletLifeTime = serializableStats.bulletLifeTime;
        AttackRange = serializableStats.attackRange;
        CurrentWeapon = serializableStats.currentWeapon;
    }

    #region Getters&Setters
    public int MaxHealth
    {
        get => stats.maxHealth;
        set
        {
            OnCharacterPropertyChanged.Invoke(PropertyID.MAXHEALTH, value);
            if (value > stats.maxHealth)
            {
                int val = value - stats.maxHealth;
                stats.maxHealth = value;
                Health += val;
            }
            else
            {
                stats.maxHealth = value;
                Health = Mathf.Min(Health, stats.maxHealth);
            }
        }
    }
    public float MaxHealthMultiplier
    {
        get => stats.multipliers.maxHealth;
        set
        {
            stats.multipliers.maxHealth = Mathf.Max(value, 0);
            OnCharacterPropertyChanged.Invoke(PropertyID.MAXHEALTHMULTIPLIER, stats.multipliers.maxHealth);
        }
    }
    public int Health
    {
        get => stats.health;
        set
        {
            stats.health = Mathf.Clamp(value, 0, MaxHealth);
            OnCharacterPropertyChanged.Invoke(PropertyID.HEALTH, stats.health);
        }
    }
    public int HealthRegeneration { get => stats.healthRegeneration; set
        {
            stats.healthRegeneration = Mathf.Max(value, 0);
            OnCharacterPropertyChanged.Invoke(PropertyID.HEALTHREGENERATION, stats.healthRegeneration);
        }
    }
    public float HealthRegenerationMultiplier
    {
        get => stats.multipliers.regeneration;
        set => stats.multipliers.regeneration = Mathf.Max(0, value);
    }
    public int BaseDamage => stats.damage;
    public int Damage
    {
        get => Mathf.RoundToInt(stats.damage*DamageMultiplier);
        set
        {
            stats.damage = value;
            OnCharacterPropertyChanged.Invoke(PropertyID.DAMAGE, Damage);
        }
    }
    public float DamageMultiplier
    {
        get => stats.multipliers.damage;
        set
        {
            stats.multipliers.damage = Mathf.Max(value, 0);
            OnCharacterPropertyChanged.Invoke(PropertyID.DAMAGEMULTIPLIER, stats.multipliers.damage);
        }
    }
    public int TemporaryDamage
    {
        get => stats.temporaryStats.damage;
        set
        {
            Damage = BaseDamage + value - TemporaryDamage;
            stats.temporaryStats.damage = value;
        }
    }
    public float CritChance
    {
        get => stats.critChance; set
        {
            stats.critChance = Mathf.Clamp(value, 0, 1);
            OnCharacterPropertyChanged.Invoke(PropertyID.CRITCHANCE, stats.critChance);
        }
    }
    public float CritMultiplier
    {
        get => stats.critMultiplier; set
        {
            stats.critMultiplier = Mathf.Max(value, 0);
            OnCharacterPropertyChanged.Invoke(PropertyID.CRITMULTIPLIER, stats.critMultiplier);
        }
    }
    public float BaseSpeed => stats.speed;
    public float Speed
    {
        get => stats.speed * SpeedMultiplier; set
        {
            stats.speed = Mathf.Max(value, 0);
            OnCharacterPropertyChanged.Invoke(PropertyID.SPEED, stats.speed);
        }
    }
    public float SpeedMultiplier
    {
        get => stats.multipliers.speed; set
        {
            stats.multipliers.speed = Mathf.Max(value, 0);
            OnCharacterPropertyChanged.Invoke(PropertyID.SPEEDMULTIPLIER, stats.multipliers.speed);
        }
    }
    public float Drag
    {
        get => stats.drag; set
        {
            stats.drag = Mathf.Max(value, 0);
            OnCharacterPropertyChanged.Invoke(PropertyID.DRAG, stats.drag);
        }
    }
    public float BaseReload => stats.reload;
    public float Reload
    {
        get => Mathf.Max(stats.reload * ReloadMultiplier, 0.05f); set
        {
            stats.reload = Mathf.Max(value, 0);
            OnCharacterPropertyChanged.Invoke(PropertyID.RELOAD, stats.reload);
        }
    }
    public float ReloadMultiplier
    {
        get => stats.multipliers.reload; set
        {
            stats.multipliers.reload = Mathf.Max(value, 0);
            OnCharacterPropertyChanged.Invoke(PropertyID.RELOADMULTIPLIER, stats.multipliers.reload);
        }
    }
    public float TemporaryReload
    {
        get => stats.temporaryStats.reload;
        set
        {
            Reload = BaseReload + value - TemporaryReload;
            stats.temporaryStats.reload = value;
        }
    }
    public float BulletSpeed
    {
        get => stats.bulletSpeed; set
        {
            stats.bulletSpeed = Mathf.Max(value, 0);
            OnCharacterPropertyChanged.Invoke(PropertyID.BULLETSPEED, stats.bulletSpeed);
        }
    }
    public float BulletLifeTime
    {
        get => stats.bulletLifeTime; set
        {
            stats.bulletLifeTime = Mathf.Max(value, 0);
            OnCharacterPropertyChanged.Invoke(PropertyID.BULLETLIFETIME, stats.bulletLifeTime);
        }
    }
    public int CurrentWeapon
    {
        get => stats.currentWeapon; set
        {
            stats.currentWeapon = Mathf.Max(value, 0);
            OnCharacterPropertyChanged.Invoke(PropertyID.CURRENTWEAPON, stats.currentWeapon);
        }
    }
    public float BulletRange
    {
        get => stats.bulletSpeed * stats.bulletLifeTime;
    }
    public float AttackRange
    {
        get => stats.attackRange;
        set => stats.attackRange = Mathf.Max(0, value);
    }
    public Sprite Sprite
    {
        get => sprite; set
        {
            if (value == null) Debug.LogError("[CharacterStats] Sprite is null");
            sprite = value;
            OnCharacterPropertyChanged.Invoke(PropertyID.SPRITE, sprite);
        }
    }
    public RuntimeAnimatorController PlayerAnimatorController
    {
        get => playerAnimatorController; set
        {
            if (value == null) Debug.LogError("[CharacterStats] RuntimeAnimatorController is null");
            playerAnimatorController = value;
            OnCharacterPropertyChanged.Invoke(PropertyID.ANIMATORCONTROLLER, playerAnimatorController);
        }
    }
    public Vector2 Position
    {
        get => position; set
        {
            position = value;
            OnCharacterPropertyChanged.Invoke(PropertyID.POSITION, position);
        }
    }
    public bool IsDead
    {
        get => stats.isDead; set
        {
            stats.isDead = value;
            OnCharacterPropertyChanged.Invoke(PropertyID.DEADNESS, stats.isDead);
        }
    }
    #endregion

}
