using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class SadGhost : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] Healthbar healthbar;
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        float spread = rb.drag * 0.3f;
        rb.drag = (Random.value * spread - spread / 2) + rb.drag;
        StartCoroutine(HideGhost());
        StartCoroutine(ChangeColor());
    }
    IEnumerator HideGhost()
    {
        float alpha = spriteRenderer.color.a;
        while (alpha > 0)
        {
            alpha -= 0.05f;
            SetAlpha(alpha);
            yield return new WaitForFixedUpdate();
        }
    }
    IEnumerator ChangeColor()
    {
        Enemy enemy = GetComponent<Enemy>();
        CharacterStats enemyStats= enemy.CharacterStats;
        Player player = Player.Instance;
        Transform _transform = enemy.Transform;
        Transform playerHitPointTransform = player.hitPoint.transform;
        while (!enemyStats.IsDead && !player.CharacterStats.IsDead)
        {
            yield return new WaitWhile(() => playerHitPointTransform != null &&
            (playerHitPointTransform.position - _transform.position).magnitude > 10f);

            if (!playerHitPointTransform) yield break;
            SetAlpha(1 - (playerHitPointTransform.position - _transform.position).magnitude / 10f);
        }
    }
    void SetAlpha(float value)
    {
        Color color;
        color = spriteRenderer.color;
        color.a = value;
        spriteRenderer.color = color;
        healthbar.SetAlpha(value);

    }
}
