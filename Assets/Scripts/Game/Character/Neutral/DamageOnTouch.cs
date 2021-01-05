using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    [SerializeField] Bullet.DamageInfo damageInfo;
    [SerializeField] bool DamageTriggers;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger == DamageTriggers)
        {
            if (collision.TryGetComponent(out TakeDamage takeDamage))
            {
                takeDamage.Damage(damageInfo, gameObject);
            }
        }
    }

    public Bullet.DamageInfo DamageInfo { get => damageInfo; set => damageInfo = value; }
}
