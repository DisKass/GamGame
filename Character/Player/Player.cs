using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(TakeDamage))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerAnimationController))]
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(Buff_Debuff_System))]
public class Player : Singleton<Player>, ICharacter
{
    [HideInInspector] private CharacterStats characterStats;
    public Point hitPoint;
    [HideInInspector] public CharacterInventory characterInventory;
    [HideInInspector] private PlayerController playerController;
    [HideInInspector] private TakeDamage takeDamage;
    [HideInInspector] public WeaponManager weaponManager;
    [HideInInspector] private Rigidbody2D rigidbody2D1;
    [HideInInspector] private SpriteRenderer spriteRenderer;
    [HideInInspector] private Animator animator;
    [HideInInspector] private Transform _transform;
    PlayerAnimationController playerAnimationController;
    Buff_Debuff_System myBuffs;

    public Events.EventPlayerLeftSafeZone OnPlayerLeftSafeZone = new Events.EventPlayerLeftSafeZone();
    public Events.EventWeaponRotated OnWeaponRotated = new Events.EventWeaponRotated();

    private Dictionary<Bullet.DamageTypeValues, uint> damageTypeCount = new Dictionary<Bullet.DamageTypeValues, uint>();
    public CharacterStats CharacterStats { get => characterStats; set => characterStats = value; }
    public Rigidbody2D Rigidbody2D { get => rigidbody2D1; set => rigidbody2D1 = value; }
    public TakeDamage TakeDamage { get => takeDamage; set => takeDamage = value; }
    public SpriteRenderer SpriteRenderer { get
        {
            //Debug.Log("[Player] SpriteRenderer Asked. IsNull: " + (spriteRenderer == null));
            return spriteRenderer;
        }
        set
        {
            //Debug.Log("[Player] SpriteRenderer Changed!");
            spriteRenderer = value;
        }
    }
    public Animator Animator { get => animator; set => animator = value; }
    public PlayerController PlayerController { get => playerController; set => playerController = value; }
    public PlayerAnimationController PlayerAnimationController { get => playerAnimationController; set => playerAnimationController = value; }
    public Buff_Debuff_System Buffs { get => myBuffs; set => myBuffs = value; }

    public Transform Transform
    {
        get => _transform;
    }

    protected override void Awake()
    {
        base.Awake();
        _transform = transform;
        Rigidbody2D = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
        PlayerAnimationController = GetComponentInChildren<PlayerAnimationController>();
        CharacterStats = GetComponent<CharacterStats>();
        playerController = GetComponent<PlayerController>();
        Buffs = GetComponent<Buff_Debuff_System>();
        CharacterStats.OnCharacterPropertyChanged.AddListener(HandleChatacterPropertyChanged);

        CharacterStats.Initialize();
        TakeDamage = GetComponent<TakeDamage>();
        damageTypeCount.Add(Bullet.DamageTypeValues.ISCRITICAL, 0);
        damageTypeCount.Add(Bullet.DamageTypeValues.ISFIRE, 0);
        damageTypeCount.Add(Bullet.DamageTypeValues.ISICE, 0);
        damageTypeCount.Add(Bullet.DamageTypeValues.ISDOT, 0);
    }
    private void Start()
    {
        TakeDamage.Initialize();
        Buffs.Initialize();
        characterInventory = CharacterInventory.Instance;
        weaponManager = WeaponManager.Instance;
        PlayerController.Initialize();
        weaponManager.OnWeaponRotated.AddListener((direction) => OnWeaponRotated.Invoke(direction));
        Load();
        weaponManager.Initialize();
        GameManager.Instance.OnStoreData.AddListener(HandleStoreData);
        StartCoroutine(Regeneration());
        //StartCoroutine(BaseRegeneration());
    }
    public void Heal(int value)
    {
        value = Mathf.RoundToInt(value * CharacterStats.HealthRegenerationMultiplier);
        if (value > 0 && CharacterStats.Health != CharacterStats.MaxHealth)
            Pop_Ups.Instance.Heal(value, Transform);
        CharacterStats.Health += value;
    }
    void HandleChatacterPropertyChanged (CharacterStats.PropertyID property, object value)
    {
        switch (property)
        {
            case CharacterStats.PropertyID.DRAG:
                Rigidbody2D.drag = (float)value;
                break;
            case CharacterStats.PropertyID.SPRITE:
                SpriteRenderer.sprite = (Sprite)value;
                break;
            case CharacterStats.PropertyID.ANIMATORCONTROLLER:
                Animator.runtimeAnimatorController = (RuntimeAnimatorController)value;
                break;
            case CharacterStats.PropertyID.DEADNESS:
                if ((bool)value)
                    Die();
                break;
            default:
                break;
        }
    }

    public void RestoreHealth()
    {
        if (CharacterStats.Health != CharacterStats.MaxHealth)
            Heal(CharacterStats.MaxHealth);
    }
    IEnumerator Regeneration()
    {
        while (!CharacterStats.IsDead)
        {
            Heal(CharacterStats.HealthRegeneration);
            yield return new WaitForSeconds(1);
        }
    }
    public void Die()
    {
        Debug.Log(CharacterStats.IsDead);
        if (CharacterStats.IsDead) return;
        Buffs.Die();
        CharacterStats.IsDead = true;
        GameManager.Instance.EndOfGame();
    }

    void HandleStoreData(GameManager.StoreDataType type, string source)
    {
        if (source == "LevelStateController" || source == "ItemPool")
        {
            //Debug.Log("[Player] type: " + type);
            if (type == GameManager.StoreDataType.SAVE) Save();
            if (type == GameManager.StoreDataType.LOAD) Load();
            //if (type == GameManager.StoreDataType.RESET) DeleteSaveFile();
        }
    }

    void Save()
    {
        CharacterStats.Position = Transform.position;
        SaveSystem<CharacterStats, CharacterStatsData>.Save(CharacterStats, "CharacterStatsData"); 
        SaveSystem<Player, DamageInfoData>.Save(this, "DamageInfoData"); 
    }
    void Load() {
        if (SaveSystem<CharacterStats, CharacterStatsData>.CheckSaveFile("CharacterStatsData"))
        {
            CharacterStatsData statsData = SaveSystem<CharacterStats, CharacterStatsData>.Load("CharacterStatsData");
            CharacterStats.SetLoadedData(statsData.serializableStats);
            Vector3 storedPosition = new Vector3(statsData.position[0], statsData.position[1]);
            Transform.position = storedPosition;
            weaponManager.SelectWeapon(CharacterStats.CurrentWeapon, Assets.Scripts.Game.Items.InitializeReason.LOADED);
        }
        else
        {
            weaponManager.SelectWeapon(CharacterStats.CurrentWeapon, Assets.Scripts.Game.Items.InitializeReason.PICKUPPED);
        }

        if (SaveSystem<Player, DamageInfoData>.CheckSaveFile("DamageInfoData"))
        {
            DamageInfoData playerDamageInfo = SaveSystem<Player, DamageInfoData>.Load("DamageInfoData");
            weaponManager.bulletInfo.DamageInfo = playerDamageInfo.damageInfo;
            damageTypeCount = playerDamageInfo.damageTypeCount;
        }
        else
        {
            weaponManager.bulletInfo.DamageInfo.UpdateInfo(Damage: Mathf.RoundToInt(CharacterStats.Damage));
        }
    }
    void DeleteSaveFile()
    {
        //Debug.Log("[Player] Reset.");
        //CharacterStats.Initialize();
    }

    public void ChangeWeapon()
    {
        CharacterStats.CurrentWeapon = 1 - CharacterStats.CurrentWeapon;
        weaponManager.SelectWeapon(CharacterStats.CurrentWeapon, Assets.Scripts.Game.Items.InitializeReason.PICKUPPED);
    }

    public Dictionary<Bullet.DamageTypeValues, uint> GetDamageTypeCount()
    {
        return damageTypeCount;
    }

    public void ChangeDamageType(Bullet.DamageTypeValues damageType, bool value)
    {
        if (damageTypeCount[damageType] == 1 && value == false) weaponManager.ChangeDamageType(damageType, value);
        if (damageTypeCount[damageType] == 0 && value == true) weaponManager.ChangeDamageType(damageType, value);
        if (value) damageTypeCount[damageType] += 1;
        if (!value) damageTypeCount[damageType] -= 1;
        if (damageTypeCount[damageType] < 0) Debug.LogError(damageType + "less than 0");
        //Debug.Log(damageType + ": " + damageTypeCount[damageType]);
    }
    public void AddDamageType(Bullet.DamageType damageType)
    {
        for (int i = 0; i < Bullet.DamageType.typesCount; i++)
        {
            if (damageType[i])
            {
                ChangeDamageType((Bullet.DamageTypeValues)i, true);
            }
        }
    }
    public void SubstractDamageType(Bullet.DamageType damageType)
    {
        for (int i = 0; i < Bullet.DamageType.typesCount; i++)
        {
            if (damageType[i])
            {
                ChangeDamageType((Bullet.DamageTypeValues)i, false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("SafeZoneTrigger"))
        {
            OnPlayerLeftSafeZone.Invoke(true);
            weaponManager.CanFire = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("SafeZoneTrigger"))
        {
            OnPlayerLeftSafeZone.Invoke(false);
            weaponManager.CanFire = true;
        }
    }

    public void Stun(bool value)
    {
        PlayerController.IsStunned = value;
    }

    //IEnumerator BaseRegeneration()
    //{
    //    while (!CharacterStats.IsDead)
    //    {
    //        if (CharacterStats.Health < CharacterStats.MaxHealth) CharacterStats.Health += CharacterStats.HealthRegeneration;
    //        yield return new WaitForSeconds(1);
    //    }
    //}
}
