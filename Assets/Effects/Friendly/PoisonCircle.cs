using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PoisonCircle : MonoBehaviour
{
    Bullet.DamageInfo damageInfo;

    private Collider2D _collider2D;
    public void Initialize()
    {
        damageInfo = WeaponManager.Instance.bulletInfo.DamageInfo;
        damageInfo.Damage = Mathf.RoundToInt(damageInfo.Damage * 0.2f);
        WeaponManager.Instance.OnDamageInfoChanged.AddListener(HandleDamageInfoChanged);

        _collider2D = GetComponent<Collider2D>();
        _collider2D.enabled = false;
        StartCoroutine(BlinkCollider());
    }

    private void HandleDamageInfoChanged(Bullet.DamageInfo damageInfo)
    {
        this.damageInfo = damageInfo;
        this.damageInfo.Damage = Mathf.RoundToInt(damageInfo.Damage * 0.2f);
    }

    IEnumerator BlinkCollider()
    {
        while (enabled)
        {
            _collider2D.enabled = true;
            yield return new WaitForSeconds(0.2f);
            _collider2D.enabled = false;
            yield return new WaitForSeconds(0.5f);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.isTrigger == true) return;
        if (collider.TryGetComponent(out TakeDamage takeDamage))
        {
            takeDamage.Damage(damageInfo, Player.Instance.gameObject);
        }
    }
}
