using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct MoveType
{
    public bool RandomMoving;
    public bool Chase;
    public bool MoveInFireDirection;
    public bool Custom;
}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyMove : MonoBehaviour
{
    [SerializeField] MoveType _moveType;
    [SerializeField] float _minIdleTime = 1f;
    [SerializeField] float _maxIdleTime = 3f;
    [SerializeField] float _minMovingTime = 1f;
    [SerializeField] float _maxMovingTime = 3f;

    [HideInInspector] public float chaseDistance = 0;
    [HideInInspector] public bool IsFrozen;
    CharacterStats enemyStats;

    Animator enemyAnimator;
    Enemy enemy;
    Transform _transform;

    Player player;

    float _idleTime;
    float _movingTime;
    bool _isIdle = true;

    Rigidbody2D _rigidbody;
    Vector2 _moveDirection;
    SpriteRenderer spriteRenderer;

    public void Initialize()
    {
        enemy = GetComponent<Enemy>();
        _transform = enemy.Transform;
        player = Player.Instance;
        LevelStateController.OnLevelStateChanged.AddListener(HadleLevelStateChanged);
        enemyStats = enemy.CharacterStats;
        if (enemyStats == null)
        {
            Debug.LogError("[EnemyMove] enemyStats is null");
        }
        _rigidbody = enemy.Rigidbody2D;
        enemyAnimator = enemy.Animator;
        spriteRenderer = enemy.SpriteRenderer;

        _rigidbody.drag = enemyStats.Drag;
        //if (!_moveType.Custom)
        //    StartCoroutine(moving());
        if (_moveType.Custom) return;
        StopAllCoroutines();

        if (_moveType.RandomMoving)
            StartCoroutine(moving());
        if (_moveType.Chase)
        {
            StartCoroutine(Chase());
        }
    }
        void HadleLevelStateChanged(LevelStateController.LevelState levelState)
        {
            if (levelState == LevelStateController.LevelState.ACTION)
            {
                if (_moveType.Custom) return;
                StopAllCoroutines();

                if (_moveType.RandomMoving)
                    StartCoroutine(moving());
                if (_moveType.Chase)
                {
                    StartCoroutine(Chase());
                }
            }
        }
    IEnumerator moving()
    { 
        while (true)
        {
            if (!IsFrozen)
                RandomMoving();
            yield return new WaitForFixedUpdate();
        }
    }
    IEnumerator Chase()
    {
        while (!enemyStats.IsDead && !player.CharacterStats.IsDead)
        {
            if (Vector3.Distance(player.Transform.position, _transform.position) > chaseDistance + 0.5f)
                MoveTo(player.Transform.position - _transform.position, enemyStats.Speed);
            else if (Vector3.Distance(player.Transform.position, _transform.position) < chaseDistance - 0.5f)
                MoveTo(_transform.position - player.Transform.position, enemyStats.Speed);
            yield return new WaitForFixedUpdate();
        }
    }
    void RandomMoving()
    {
        float currentTime = Time.time;
        if (_isIdle)
        {
            if (_idleTime > currentTime) return;
            else // Переход в статус ходьбы
            {
                _isIdle = false;
                ChangeDirection();
                spriteRenderer.flipX = _moveDirection.x > 0 ? false : true;
                _movingTime = currentTime + UnityEngine.Random.Range(_minMovingTime, _maxMovingTime);
                enemyAnimator.SetBool("Move", true);
            }
        }
        if (_movingTime > currentTime)
        {
            MoveTo(_moveDirection, enemyStats.Speed);
        }
        else
        {
            _isIdle = true;
            _idleTime = currentTime + UnityEngine.Random.Range(_minIdleTime, _maxIdleTime);
            enemyAnimator.SetBool("Move", false);
        }
    }

    public void Escape(Transform target, float speed, float range)
    {
        StartCoroutine(Escape(speed, target, range));
    }
    IEnumerator Escape(float speed, Transform target, float range)
    {
        while (!enemyStats.IsDead)
        {
            if (IsFrozen) yield return new WaitWhile(() => IsFrozen);
            if (target == null || enemy.Transform == null) yield break;
            if (Vector2.Distance(enemy.Transform.position, target.position) < range)
                MoveTo(enemy.Transform.position - target.position, speed);
            yield return new WaitForFixedUpdate();
        }
    }
    public void MoveTo(Vector2 direction, float speed)
    {
        direction = direction.normalized;
        _rigidbody.AddForce(speed * _rigidbody.mass * _rigidbody.drag * Time.deltaTime * 60 * direction); // 60 = fps
    }
    void ChangeDirection()
    {
        _moveDirection = UnityEngine.Random.insideUnitCircle;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        LevelStateController.OnLevelStateChanged.RemoveListener(HadleLevelStateChanged);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!_isIdle)
            ChangeDirection();
    }
}