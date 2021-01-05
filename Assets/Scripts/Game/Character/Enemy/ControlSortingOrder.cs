using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class ControlSortingOrder : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    Transform _transform;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _transform = transform;
    }
    private void Update()
    {
        _spriteRenderer.sortingOrder = -(Mathf.RoundToInt(_transform.position.y*10));
    }
}
