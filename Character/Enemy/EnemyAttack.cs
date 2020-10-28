using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//enum AttackType
//{
//    PEACEFUL,
//    CUSTOM,
//    FIRE
//}

[System.Serializable] 
struct AttackType
{
    public bool Peaceful;
    public bool Shoot;
    public bool Touch;
    public bool Jump;
    public bool Custom;
}
[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(Collider2D))]
public class EnemyAttack : MonoBehaviour
{
    [SerializeField] AttackType attackType;
    CharacterStats enemyStats;
    [SerializeField] public Point[] firePoint;
    [SerializeField] Sprite bulletSprite;

    [HideInInspector] public bool IsFrozen;

    [SerializeField] Bullet.DamageType damageType;
    [SerializeField] float punchPower = 0;
    protected Bullet.BulletInfo bulletInfo;
    protected Bullet.DamageInfo damageInfo;
    Enemy enemy;

    Collider2D colliderForTouches;

    Player player;
    Point playerHitPoint;
    float jumpForce = 3;

    float lastShot = 0;
    Coroutine CurrentAttackCoroutine;

    ObjectsPool objectPool;


    public void Initialize()
    {
        if (attackType.Peaceful) return;

        enemy = GetComponent<Enemy>();
        player = Player.Instance;
        playerHitPoint = player.hitPoint;
        enemyStats = enemy.CharacterStats;
        //Debug.Log("[EnemyAttack] range: " + enemyStats.BulletRange);
        enemyStats.OnCharacterPropertyChanged.AddListener(HandleCharacterPropertyChanged);
        damageInfo.UpdateInfo(Damage: Mathf.RoundToInt(enemyStats.Damage), damageType: damageType);
        bulletInfo.UpdateInfo(sourceTransform: transform, Damage: damageInfo, Speed: enemyStats.BulletSpeed,
            LifeTime: enemyStats.BulletLifeTime, CritChance: enemyStats.CritChance,
            CritMultiplier: enemyStats.CritMultiplier, penetration: false, Sprite: bulletSprite);

        if (attackType.Shoot)
            CurrentAttackCoroutine = StartCoroutine(Shoot());
        if (attackType.Jump)
            StartCoroutine(Jump());

    }
    void HandleCharacterPropertyChanged(CharacterStats.PropertyID property, object value)
    {
        switch (property)
        {
            case CharacterStats.PropertyID.DAMAGE:
            case CharacterStats.PropertyID.DAMAGEMULTIPLIER:
                bulletInfo.DamageInfo.Damage = enemyStats.Damage;
                break;
            case CharacterStats.PropertyID.CRITCHANCE:
            case CharacterStats.PropertyID.CRITMULTIPLIER:
            case CharacterStats.PropertyID.BULLETSPEED:
            case CharacterStats.PropertyID.BULLETLIFETIME:
                bulletInfo.UpdateProperty(property, value);
                break;
            case CharacterStats.PropertyID.RELOAD:
            case CharacterStats.PropertyID.RELOADMULTIPLIER:
                if (CurrentAttackCoroutine == null) return;
                StopCoroutine(CurrentAttackCoroutine);
                CurrentAttackCoroutine = StartCoroutine(Shoot());
                break;
            default:
                break;
        }
    }
    public void Fire(Vector2 startPosition, Vector2 direction)
    {
        if (IsFrozen) return;
        bulletInfo.Rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, direction));
        bulletInfo.StartPosition = startPosition;
        FireManager.Instance.Fire(bulletInfo);
    }
    public void Fire(Vector2 startPosition, Quaternion rotation)
    {
        if (IsFrozen) return;
        bulletInfo.Rotation = rotation;
        bulletInfo.StartPosition = startPosition;
        FireManager.Instance.Fire(bulletInfo);
    }
    public void MultyDirectionalFire(params Vector2[] direction)
    {
        if (IsFrozen) return;
        bulletInfo.StartPosition = firePoint[0].Position;
        for (int i = 0; i < direction.Length; i++)
        {
            //bulletInfo.Direction = direction[i];
            bulletInfo.Rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, direction[i]));
            FireManager.Instance.Fire(bulletInfo);
        }
    }
    IEnumerator Shoot()
    {
        while (!player.CharacterStats.IsDead && enabled)
        {
            if (IsFrozen) yield return new WaitWhile(() => IsFrozen);
            RaycastHit2D hit = RaycastToPlayer();
            if (hit.collider != null && hit.collider.tag == "Player")
            {
                for (int i = 0; i < firePoint.Length; i++)
                    Fire(firePoint[i].Position, (playerHitPoint.Position - firePoint[i].Position).normalized);
            }
            yield return new WaitForSeconds(enemyStats.Reload);
        }
    }
    IEnumerator Jump()
    {
        Transform _transform = enemy.Transform;
        while (!enemyStats.IsDead && !player.CharacterStats.IsDead)
        {
            yield return new WaitWhile(() => playerHitPoint != null &&
            (playerHitPoint.Position - (Vector2)_transform.position).magnitude > enemyStats.AttackRange);

            if (!playerHitPoint) yield break;
            Jump(playerHitPoint.Position - (Vector2)_transform.position, enemy.Rigidbody2D.mass * enemyStats.AttackRange * enemy.CharacterStats.Drag);
            yield return new WaitForSeconds(0.7f);
        }
    }
    public RaycastHit2D RaycastToPlayer()
    {
        return RaycastToPlayer(LayerMask.GetMask(new string[] { "Player", "SafeZone" }));
    }
    public RaycastHit2D RaycastToPlayer(LayerMask layerMask)
    {
        return Physics2D.Raycast(firePoint[0].Position, playerHitPoint.Position - firePoint[0].Position, enemyStats.BulletRange, layerMask);
    }
    public void Jump(Vector2 direction, float force)
    {
        enemy.Rigidbody2D.AddForce(force * direction.normalized, ForceMode2D.Impulse);
    }
    public void  InitializeObjectPool(GameObject objectToSpawn, int maxCount)
    {
        objectPool = new ObjectsPool(objectToSpawn, maxCount);
    }
    public void SpawnObject()
    {
        GameObject instantiatedObject;
        for (int i = 0; i < firePoint.Length; i++)
        {
            instantiatedObject = objectPool.GetElement();
            if (instantiatedObject == null) return;
            instantiatedObject.transform.position = firePoint[i].Position;
            if (instantiatedObject.TryGetComponent(out Enemy e))
            {
                e.Initialize();
                e.IsSummoned = true;
            }
        }
    }
    public void SpawnObjectOverTime(float minTime, float maxTime = 0)
    {
        StartCoroutine(SpawnOverTime(minTime, maxTime));
    }
    IEnumerator SpawnOverTime(float minTime, float maxTime = 0)
    {
        while (true)
        {
            if (maxTime == 0)
                yield return new WaitForSeconds(minTime);
            else
                yield return new WaitForSeconds(Random.Range(minTime, maxTime));
            SpawnObject();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!attackType.Touch) return;
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.TryGetComponent(out TakeDamage td))
            {
                td.Damage(damageInfo, gameObject);
                collision.attachedRigidbody.AddForce((collision.transform.position - transform.position).normalized * punchPower);
            }
        }
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
