using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ShadowExplode : MonoBehaviour
{
    Bullet.DamageInfo damageInfo;
    bool IsExplosion = false;
    GameObject target;
    static float Reload = 1f;
    static float lastExplode = 0f;
    SpriteRenderer spriteRenderer;

    static public bool CanExplode { get => lastExplode + Reload <= Time.time; }
    public void Attack(Bullet.DamageInfo damageInfo, GameObject target)
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        if (!CanExplode) return;
        this.damageInfo = damageInfo;
        this.target = target;
        GetComponent<CircleCollider2D>().enabled = true;
        StartCoroutine(CheckExplode());
    }
    private void Explode()
    {
        if (IsExplosion) return;
        IsExplosion = true;

        target.GetComponent<TakeDamage>().Damage(damageInfo, Player.Instance.gameObject);
        lastExplode = Time.time;
        spriteRenderer.enabled = true;
        GetComponent<Animation>().Play();
    }
    IEnumerator CheckExplode()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        if (!IsExplosion) EndExplosion();
    }
    void EndExplosion()
    {
        spriteRenderer.enabled = false;
        IsExplosion = false;
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        GetComponent<CircleCollider2D>().enabled = false;
        if (other.gameObject == target) return;
        if (other.TryGetComponent(out TakeDamage td))
        {
            if (other.isTrigger != true)
            {
                td.Damage(damageInfo, Player.Instance.gameObject);
                Explode();
            }
        }
    }
}
