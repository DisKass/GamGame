using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class SpikesTrap : MonoBehaviour
{
    [SerializeField] Sprite hiddenSpikes;
    [SerializeField] Sprite showesSpikes;
    [SerializeField] bool spikesEnabled;
    SpriteRenderer _spriteRenderer;
    Collider2D _collider;

    [SerializeField]
    Bullet.DamageInfo damageInfo;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        StartCoroutine(Active());
    }

    IEnumerator Active()
    {
        while (true)
        {
            if (spikesEnabled)
            {
                spikesEnabled = false;
                _spriteRenderer.sprite = hiddenSpikes;
                _collider.enabled = false;
                yield return new WaitForSeconds(2f);
            }
            else
            {
                spikesEnabled = true;
                _spriteRenderer.sprite = showesSpikes;
                _collider.enabled = true;
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && other.isTrigger == false && other.TryGetComponent(out TakeDamage td))
        {
            td.Damage(damageInfo, gameObject);
        }
    }
}
