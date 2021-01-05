using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatIcyMan : MonoBehaviour
{
    Enemy enemy;

    [SerializeField] int shotsCount = 8;
    [SerializeField] float fireRate = 0.1f;
    [SerializeField] float spreadAngle = 45f;
    [SerializeField] float checkPlayerRate = 0.1f;
    EnemyAttack enemyAttack;
    Player player;
    Point[] firePoint;
    int CurrentFirePoint = 0;
    private void Start()
    {
        enemy = GetComponent<Enemy>();
        enemyAttack = enemy.enemyAttack;
        player = Player.Instance;
        firePoint = enemyAttack.firePoint;
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        while (!enemy.CharacterStats.IsDead && enabled)
        {
            RaycastHit2D hit = enemyAttack.RaycastToPlayer();
            if (hit.collider == null)
            {
                yield return new WaitForSeconds(checkPlayerRate);
                continue;
            }
            if (hit.collider.gameObject == player.gameObject)
            {
                float angle;
                for (int i = 0; i < shotsCount; i++)
                {
                    angle = Vector2.SignedAngle(Vector2.right, player.hitPoint.Position - firePoint[CurrentFirePoint].Position);
                    enemyAttack.Fire(firePoint[CurrentFirePoint].Position, Quaternion.Euler(0, 0, angle + Random.value * spreadAngle - spreadAngle/2f));
                    ChoseNextFirePoint();
                    yield return new WaitForSeconds(fireRate);

                }
            }
            yield return new WaitForSeconds(enemy.CharacterStats.Reload);
        }
    }

    void ChoseNextFirePoint()
    {
        CurrentFirePoint++;
        if (CurrentFirePoint == firePoint.Length) CurrentFirePoint = 0;
    }
}
