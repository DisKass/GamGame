using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(ParticleSystem))]
public class FireOrb : MonoBehaviour
{
    Player player;
    float bulletSpeed = 35f;
    float lifeTime = 0.5f;

    Vector3 startPosition;
    ParticleSystem ParticleSystem;
    Bullet.DamageInfo damageInfo;
    Transform _transform;
    Transform parent;
    public bool IsWait { get; set; } = true;
    private void Awake()
    {
        player = Player.Instance;
        _transform = transform;
        startPosition = _transform.localPosition;
        ParticleSystem = GetComponent<ParticleSystem>();
        parent = _transform.parent;
    }
    public void Fire(Bullet.DamageInfo damageInfo, Vector2 direction)
    {
        IsWait = false;
        this.damageInfo = damageInfo;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Rigidbody2D>().simulated = true;
        GetComponent<Rigidbody2D>().velocity += direction.normalized * bulletSpeed;
        _transform.SetParent(parent.parent);
        StartCoroutine(WaitEndOfMoving());
    }
    public void SetActiveParticles(bool active)
    {
        if (active) ParticleSystem.Play();
        else ParticleSystem.Stop();
    }
    IEnumerator WaitEndOfMoving()
    {
        yield return new WaitForSeconds(lifeTime);
        Restore();
    }
    public void Restore()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<Collider2D>().enabled = false;
        _transform.SetParent(parent);
        ParticleSystem.Stop();
        _transform.localPosition = startPosition;
        ParticleSystem.Play();
        IsWait = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out TakeDamage td))
        {
            if (other.isTrigger == true)
            {
                td.Damage(damageInfo, player.gameObject);
            }
        }
    }
}
