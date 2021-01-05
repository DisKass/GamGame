using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(ParticleSystem))]
public class StasisCircle : MonoBehaviour
{
    Bullet.DamageInfo damageInfo;
    Animation myAnimation;
    Collider2D myCollider;
    ParticleSystem myParticleSystem;
    bool initialized = false;
    SpriteRenderer spriteRenderer;
    float stasisTime;
    Transform _transform;
    public Events.EventHitPlayer OnHitPlayer = new Events.EventHitPlayer();

    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        initialized = true;
        _transform = transform;
        myAnimation = GetComponent<Animation>();
        myCollider = GetComponent<Collider2D>();
        myParticleSystem = GetComponent<ParticleSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Activate(float stasisTime, Vector3 position)
    {
        if (!initialized)
        {
            Initialize();
            initialized = true;
        }
        this.stasisTime = stasisTime;
        myAnimation.Stop();
        myAnimation.Play();
        _transform.position = position;
        //myAnimation.Play();
        //StartCoroutine(BlinkCollider());
    }
    void Explode()
    {
        StopAllCoroutines();
        StartCoroutine(BlinkCollider());
    }
    IEnumerator BlinkCollider()
    {
        myCollider.enabled = true;
        Color color = spriteRenderer.color;
        color.a = 1;
        spriteRenderer.color = color;
        myParticleSystem.Play();
        OnHitPlayer.Invoke();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        myCollider.enabled = false;

        yield return new WaitForSeconds(stasisTime);
        myParticleSystem.Stop();
        color.a = 0;
        spriteRenderer.color = color;
    }
   

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Conntact");
        if (other.TryGetComponent(out Buff_Debuff_System td))
        {
            if (other.isTrigger == false)
            {
                td.ActivateStasis(stasisTime);
            }
        }
    }
}
