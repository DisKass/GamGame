using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class ToxicMine : MonoBehaviour
{
    [SerializeField] Sprite defaultMine;
    [SerializeField] Sprite triggeredMine;
    [SerializeField] Sprite bulletSprite;
    SpriteRenderer _spriteRenderer;
    Collider2D _collider;
    float timer = 2f;
    float timeWithChangedSprite = 0.5f;

    Transform _transform;

    [SerializeField]
    Bullet.DamageInfo damageInfo;
    Bullet.BulletInfo bulletInfo;

    
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _transform = transform;
        bulletInfo = new Bullet.BulletInfo();
        bulletInfo.UpdateInfo(sourceTransform: _transform, Damage: damageInfo, Speed: 20,
            LifeTime: 2f, Sprite: bulletSprite);
    }
    private void OnEnable()
    {
        StartCoroutine(Active());
    }
    IEnumerator Active()
    {
        yield return new WaitForSeconds(timer - timeWithChangedSprite);
        _spriteRenderer.sprite = triggeredMine;
        yield return new WaitForSeconds(timeWithChangedSprite);
        bulletInfo.StartPosition = _transform.position;
        for (int i = 0; i < 8; i++)
        {
            bulletInfo.Rotation = Quaternion.Euler(0, 0, 45 * i);
            FireManager.Instance.Fire(bulletInfo);
        }
        Restore();
    }
    void Restore()
    {
        StopAllCoroutines();
        _spriteRenderer.sprite = defaultMine;
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && other.isTrigger == false && other.TryGetComponent(out TakeDamage td))
        {
            damageInfo.CritChance = 1;
            damageInfo.CritMultiplier = 2;
            td.Damage(damageInfo, gameObject);
            damageInfo = bulletInfo.DamageInfo;
            Restore();
        }
    }
}
