using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explore : MonoBehaviour
{
    [SerializeField] private bool IsTrap;
    [SerializeField] private Animation exploreAnimation;
    [SerializeField] private Bullet.DamageInfo damageInfo;
    public GameObject damageSource;
    bool IsReady { get; set; } = true;

    private void Awake()
    {
        if (IsTrap) damageSource = gameObject;
    }
    public void Reload()
    {
        IsReady = true;
        gameObject.SetActive(false);
    }
    public void Activate()
    {
        if (IsReady)
        {
            exploreAnimation.Play();
            IsReady = false;
        }
    }


    IEnumerator ActivateEveryTime(float time)
    {
        float lastActivationTime = 0;
        while (enabled)
        {
            Activate();
            lastActivationTime = Time.time;
            yield return new WaitUntil(() => IsReady);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger == false)
        {
            if (collision.TryGetComponent(out TakeDamage takeDamage))
            {
                if (damageSource == null)
                {
                    throw new System.Exception("Source of damage not specified");
                }
                takeDamage.Damage(damageInfo, damageSource);
            }
        }
    }

}
