using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableByButton : MonoBehaviour
{
    [SerializeField] GameObject target;

    public void ChangeActive()
    {
        target.SetActive(!target.activeSelf);
    }
}
