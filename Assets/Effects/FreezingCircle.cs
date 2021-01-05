using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(SpriteRenderer))]
public class FreezingCircle : MonoBehaviour
{
    GameObject source;
    Bullet.DamageInfo damageInfo;
    Animation myAnimation;
    Collider2D myCollider;
    ParticleSystem myParticleSystem;
    bool initialized = false;
    public void Initialize()
    {
        Color color = GetComponent<SpriteRenderer>().color;
        color.a = 0;
        GetComponent<SpriteRenderer>().color = color;
        myAnimation = GetComponent<Animation>();
        myCollider = GetComponent<Collider2D>();
        myParticleSystem = GetComponent<ParticleSystem>();
    }
    public void Activate(Bullet.DamageInfo damageInfo, GameObject source)
    {
        if (!initialized)
        {
            Initialize();
            initialized = true;
        }
        this.source = source;
        this.damageInfo = damageInfo;
        myParticleSystem.Play();
        myAnimation.Play();
        StartCoroutine(BlinkCollider());
    }
    IEnumerator BlinkCollider()
    {
        myCollider.enabled = true;
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        myCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out TakeDamage td))
        {
            if (other.isTrigger == false)
            {
                td.Damage(damageInfo, source);
            }
        }
    }
}
