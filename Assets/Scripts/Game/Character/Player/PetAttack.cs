using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PetAttack : MonoBehaviour
{
    [SerializeField] AttackType attackType;
    [SerializeField] Bullet.DamageInfo damageInfo;
    [SerializeField] Collider2D checkCollider;
    [SerializeField] Collider2D attackCollider;
    [SerializeField] PetMove petMove;

    bool IsAttacking = false;
    float AttackTime = 0.6f;
    Player player;
    Rigidbody2D _rigidbody2D;
    Transform _transform;
    float jumpReload = 1f;

    public void Initialize()
    {
        player = Player.Instance;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
        checkCollider.enabled = true;
        attackCollider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (IsAttacking)
        {
            if (collider2D.isTrigger == false) return;
            if (attackCollider.IsTouching(collider2D) == false) return;
            if (collider2D.TryGetComponent(out TakeDamage takeDamage))
            {
                takeDamage.Damage(damageInfo, player.gameObject);
            }
        }
        else
        if (attackType.Jump)
        {
            Jump(collider2D.transform.position - _transform.position, 5 * _rigidbody2D.drag);
            StartCoroutine(Attacking());
            StartCoroutine(ActivateCollider(jumpReload));
        }
    }
    IEnumerator Attacking()
    {
        checkCollider.enabled = false;
        attackCollider.enabled = true;
        petMove.IsPaused = true;
        IsAttacking = true;
        yield return new WaitForSeconds(AttackTime);

        attackCollider.enabled = false;
        IsAttacking = false;
    }
    IEnumerator ActivateCollider(float time)
    {
        yield return new WaitForSeconds(time);
        petMove.IsPaused = false;
        checkCollider.enabled = true;
    }
    void Jump(Vector3 direction, float force)
    {
        _rigidbody2D.AddForce(force * direction.normalized, ForceMode2D.Impulse);
    }
}
