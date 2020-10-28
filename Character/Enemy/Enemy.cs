using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(EnemyMove))]
[RequireComponent(typeof(EnemyAttack))]
[RequireComponent(typeof(Buff_Debuff_System))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(TakeDamage))]
[RequireComponent(typeof(Transform))]
public class Enemy : MonoBehaviour, ICharacter
{
    private CharacterStats enemyStats;
    [SerializeField] protected GameObject EnemyDeathEffectPrefab;
    protected GameObject EnemyDeathEffect;
    [HideInInspector] public EnemyAttack enemyAttack;
    [HideInInspector] public EnemyMove enemyMove;
    [HideInInspector] private TakeDamage takeDamage;
    private Rigidbody2D _rigidbody2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Buff_Debuff_System buffs;
    private Transform _transform;
    public bool IsSummoned = false;

    public static Events.EventEnemySpawned OnEnemySpawned = new Events.EventEnemySpawned();
    public static Events.EventEnemyDespawned OnEnemyDie = new Events.EventEnemyDespawned();

    static public bool CanSpawn { get; set; } = true;
    public CharacterStats CharacterStats { get => enemyStats; set => enemyStats = value; }
    public Rigidbody2D Rigidbody2D { get => _rigidbody2D; set => _rigidbody2D = value; }
    public Animator Animator { get => animator; set => animator = value; }
    public SpriteRenderer SpriteRenderer { get => spriteRenderer; set => spriteRenderer = value; }
    public TakeDamage TakeDamage { get => takeDamage; set => takeDamage = value; }
    public Buff_Debuff_System Buffs { get => buffs; set => buffs = value; }
    public Transform Transform { get => _transform; }
    private bool initialized = false;
    protected void Awake()
    {
        if (!CanSpawn)
        {
            Debug.Log("!CanSpawn");
            Destroy(gameObject);
            return;
        }
        CharacterStats = GetComponent<CharacterStats>();
        CharacterStats.Initialize();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        buffs = GetComponent<Buff_Debuff_System>();
        Animator = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        enemyAttack = GetComponent<EnemyAttack>();
        enemyMove = GetComponent<EnemyMove>();
        TakeDamage = GetComponent<TakeDamage>();
        _transform = transform;
        buffs.OnCharacterStunned.AddListener(Stun);
    }
    virtual protected void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        if (!CanSpawn)
        {
            Destroy(gameObject);
            return;
        }
        if (initialized) return;
        CharacterStats.Initialize();
        TakeDamage.Initialize();
        Buffs.Initialize();
        Animator.runtimeAnimatorController = CharacterStats.PlayerAnimatorController;
        enemyAttack.Initialize();
        enemyMove.Initialize();
        IsSummoned = false;
        initialized = true;
        OnEnemySpawned.Invoke(this);
    }

    virtual public void Die()
    {
        if (CharacterStats.IsDead) return;
        Buffs.Die();
        EnemyDeathEffect = Instantiate(EnemyDeathEffectPrefab);
        EnemyDeathEffect.GetComponent<Dissolve>().Initialize(gameObject);
        CharacterStats.IsDead = true;
        gameObject.SetActive(false);
        OnEnemyDie.Invoke(this);
        initialized = false;
    }

    public void Stun(bool value)
    {
        enemyMove.IsFrozen = value;
        enemyAttack.IsFrozen = value;
        Animator.speed = value ? 0 : 1;
    }
}
