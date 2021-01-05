using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class BatBlob : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    int count = 5;
    GameObject[] bullets;
    [SerializeField] Transform bulletsContainer;
    void Start()
    {
        bullets = new GameObject[count];
        float angle = 360f / count;
        Vector2 bulletPosition = new Vector3(5, 0, 0);
        GetComponent<EnemyMove>().chaseDistance = 5f;
        for (int i = 0; i < count; i++)
        {
            bullets[i] = Instantiate(bulletPrefab, bulletsContainer);
            bullets[i].transform.localPosition = bulletPosition;
            bullets[i].transform.RotateAround(bulletsContainer.position, bulletsContainer.forward, angle * i);
        }
        StartCoroutine(RotateBullets());
    }

    IEnumerator RotateBullets()
    {
        Vector3 angle = new Vector3(0, 0, 5f);
        while (enabled)
        {
            bulletsContainer.Rotate(angle);
            yield return new WaitForFixedUpdate();
        }
    }
}
