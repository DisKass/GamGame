using UnityEngine;
using Assets.Scripts.Game.Items;

public class WeaponManager : Singleton<WeaponManager>
{
    CharacterStats playerStats;

    [SerializeField] GameObject[] _weaponPrefabs;
    [HideInInspector] public Weapon _currentWeapon;
    float lastShot = 0f;

    Bullet.DamageType damageType;
    public Bullet.BulletInfo bulletInfo;
    private Transform _transform;
    private FireManager fireManager;
    private float reload;
    public Events.EventDamageTypeChanged OnDamageTypeChanged = new Events.EventDamageTypeChanged();
    public Events.EventDamageInfoChanged OnDamageInfoChanged = new Events.EventDamageInfoChanged();
    public Events.EventWeaponRotated OnWeaponRotated = new Events.EventWeaponRotated();
    public Events.EventPlayerFired OnPlayerFired = new Events.EventPlayerFired();
    Player player;
    public bool CanFire { get; set; } = true;

    public float spread = 0;

    public int sortingOrder
    {
        set
        {
            if (_currentWeapon != null)
                _currentWeapon.GetComponent<SpriteRenderer>().sortingOrder = value;
        }
    }
    public void ChangeDamageType(Bullet.DamageTypeValues damageType, bool value)
    {
        bulletInfo.ChangeDamageType(damageType, value);
        OnDamageTypeChanged.Invoke(damageType, value);
        OnDamageInfoChanged.Invoke(bulletInfo.DamageInfo);
    }
    protected override void Awake()
    {
        base.Awake();
        _transform = transform;
    }
    public void Initialize()
    {
        player = Player.Instance;
        player.PlayerController.OnPlayerLook.AddListener(HandlePlayerLook);
        playerStats = player.CharacterStats;
        playerStats.OnCharacterPropertyChanged.AddListener(HandleCharacterPropertyChanged);
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
        //SelectWeapon(playerStats.CurrentWeapon, InitializeReason.PICKUPPED);
        bulletInfo.UpdateInfo(sourceTransform: player.transform, Damage: bulletInfo.DamageInfo, Speed: playerStats.BulletSpeed,
            LifeTime: playerStats.BulletLifeTime, CritChance: playerStats.CritChance, CritMultiplier: playerStats.CritMultiplier,
            penetration: false, Sprite: _currentWeapon.BulletSprite);
        //bulletInfo.OnBulletDestroyed = (bulletInfo, position) =>
        //{
        //    bulletInfo.StartPosition = position;
        //    bulletInfo.OnBulletDestroyed = null;
        //    bulletInfo.Rotation = Quaternion.Euler(bulletInfo.Rotation.eulerAngles + Vector3.forward * 180);
        //    fireManager.Fire(bulletInfo);
        //};

        fireManager = FireManager.Instance;
    }


    void HandleCharacterPropertyChanged(CharacterStats.PropertyID property, object value)
    {
        switch (property)
        {
            case CharacterStats.PropertyID.DAMAGEMULTIPLIER:
            case CharacterStats.PropertyID.DAMAGE:
                bulletInfo.DamageInfo.Damage = playerStats.Damage;
                OnDamageInfoChanged.Invoke(bulletInfo.DamageInfo);
                break;
            case CharacterStats.PropertyID.BULLETLIFETIME:
                bulletInfo.LifeTime = (float)value;
                break;
            case CharacterStats.PropertyID.BULLETSPEED:
                bulletInfo.Speed = (float)value;
                break;
            case CharacterStats.PropertyID.CRITCHANCE:
                bulletInfo.DamageInfo.CritChance = (float)value;
                OnDamageInfoChanged.Invoke(bulletInfo.DamageInfo);
                break;
            case CharacterStats.PropertyID.CRITMULTIPLIER:
                bulletInfo.DamageInfo.CritMultiplier = (float)value;
                OnDamageInfoChanged.Invoke(bulletInfo.DamageInfo);
                break;
        }
    }

    public void SelectWeapon(int number, InitializeReason reason)
    {
        GameObject _currentWeaponObject;
        if (_currentWeapon != null)
        {
            _currentWeapon.Deinitialize();
            Destroy(_currentWeapon.gameObject);
        }
        _currentWeaponObject = Instantiate(_weaponPrefabs[number]);
        _currentWeaponObject.transform.parent = _transform;
        _currentWeaponObject.transform.localPosition = Vector3.zero;
        _currentWeaponObject.transform.rotation = _transform.rotation;
        _currentWeaponObject.transform.localScale = Vector3.one;

        _currentWeapon = _currentWeaponObject.GetComponent<Weapon>();
        bulletInfo.Sprite = _currentWeapon.BulletSprite;
        _currentWeapon.Initialize(reason);
    }

    void HandlePlayerLook(Vector2 value)
    {
        SetRotation(Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, value.normalized)), value.x > 0);
    }
    public void SetRotation(Quaternion rotation, bool isFacingRight)
    {
        if (!isFacingRight)
        {
            rotation *= Quaternion.Euler(180f, 0, 0);
        }
        _currentWeapon.SetRotation(rotation);
        OnWeaponRotated.Invoke(_currentWeapon.Direction);
        //if (!isFacingRight)
        //{
        //    _transform.Rotate(180f, 0, 0);
        //}
    }

    public void Fire()
    {
        if (!CanFire) return;
        if (Time.time - lastShot > playerStats.Reload)
        {
            //ShowInformation.Instance.AddMessage(_currentWeapon.transform.localRotation.eulerAngles.z.ToString());
            lastShot = Time.time;
            OnPlayerFired.Invoke();
            float currentSpread;
            for (int i = 0; i < _currentWeapon._bulletSpawnPoints.Count; i++)
            {
                bulletInfo.StartPosition = _currentWeapon._bulletSpawnPoints[i].transform.position;
                bulletInfo.Rotation = _currentWeapon._bulletSpawnPoints[i].transform.rotation;
                if (spread > 0)
                {
                    currentSpread = Random.value * spread - spread/2f;
                    bulletInfo.Rotation *= Quaternion.Euler(0, 0, currentSpread);
                }

                fireManager.Fire(bulletInfo);
            }
        }
    }

    private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        //if (currentState == GameManager.GameState.PREGAME) _currentWeapon.Deinitialize();
    }
}
