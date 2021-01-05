using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    private void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.right, ForceMode2D.Impulse);
    }

}
