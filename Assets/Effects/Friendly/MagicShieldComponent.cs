using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MagicShieldComponent : MonoBehaviour
{
    float reload = 0.3f;
    float duration = 0.2f;
    bool IsReadyToActivate = true;

    SpriteRenderer _spriteRenderer;
    Buff_Debuff_System playerBuffs;
    public void Initialize()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        playerBuffs = Player.Instance.Buffs;
        _spriteRenderer.enabled = false;
    }
    public void Activate()
    {
        if (IsReadyToActivate)
            StartCoroutine(ActivateShield());
    }

    IEnumerator ActivateShield()
    {
        IsReadyToActivate = false;
        _spriteRenderer.enabled = true;
        playerBuffs.IsImmune = true;
        yield return new WaitForSeconds(duration);
        _spriteRenderer.enabled = false;
        playerBuffs.IsImmune = false;
        yield return new WaitForSeconds(reload);
        IsReadyToActivate = true;
    }
}
