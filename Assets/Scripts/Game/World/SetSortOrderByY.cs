using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SetSortOrderByY : MonoBehaviour
{
    void Start()
    {
        Set();
    }
    public void Set()
    {
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(-transform.position.y * 10);
    }
}
