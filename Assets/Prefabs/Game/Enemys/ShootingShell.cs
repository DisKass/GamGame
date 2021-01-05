using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingShell : MonoBehaviour
{
    GameArea GameArea;
    Transform _transform;
    Point playerHitPoint;
    Buff_Debuff_System Buffs;
    private void Awake()
    {
        _transform = transform;
        Buffs = GetComponent<Enemy>().Buffs;
    }
    void Start()
    {
        playerHitPoint = Player.Instance.hitPoint;
        GameArea = GameArea.Instance;
        StartCoroutine(Active());
    }

    IEnumerator Active()
    {
        while (enabled)
        {
            StartCoroutine(Fade(0, 0.1f));
            yield return new WaitForSeconds(2);
            StartCoroutine(Fade(1, 0.1f));
            yield return new WaitForSeconds(2);
        }
    }
    IEnumerator Fade(float value, float step)
    {
        Vector2 scale = _transform.localScale;
        float sign = Mathf.Sign(value - scale.y);
        while (_transform.localScale.y != value)
        {
            if (Mathf.Abs(value - scale.y) < step)
            {
                scale.y = value;
            }
            else scale.y += step * sign;
            _transform.localScale = scale;
            yield return new WaitForSeconds(0.01f);
        }
        if (value == 0)
        {
            Buffs.Freeze(19);
            _transform.position = GameArea.GetRandomPointInWorld();
        }
    }
}
