using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenColumn : MonoBehaviour
{
    [SerializeField] bool IsActive = true;
    [SerializeField] FreezingCircle freezingCircle;
    [SerializeField] float frequency;
    [SerializeField] Bullet.DamageInfo damageInfo;
    void Start()
    {
        freezingCircle.Initialize();
        StartCoroutine(Active());
    }

    IEnumerator Active()
    {
        while (IsActive)
        {
            freezingCircle.Activate(damageInfo, gameObject);
            yield return new WaitForSeconds(frequency);
        }
    }
    private void OnDestroy()
    {
        IsActive = false;
    }
}
