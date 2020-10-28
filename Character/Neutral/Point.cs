using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    [HideInInspector] public Transform _transform;

    public Vector2 Position => IsStatic ? staticPosition : (Vector2)_transform.position;
    public Vector2 Direction => IsStatic ? staticDirection : (Vector2)_transform.right;
    bool IsStatic = false;
    Vector2 staticPosition;
    Vector2 staticDirection;

    void Awake()
    {
        _transform = transform;
        IsStatic = gameObject.isStatic;
        staticPosition = _transform.position;
        staticDirection = _transform.right;
    }
}
