using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : MonoBehaviour
{
    [SerializeField] GameObject littleBlob;
    Enemy enemy;
    int maxLittleBlobsCount = 3;
    float minLittleBlobsSpawnRate = 3;
    float maxLittleBlobsSpawnRate = 5;
    private void Start()
    {
        Enemy.OnEnemyDie.AddListener(Die);
        enemy = GetComponent<Enemy>();
        enemy.enemyAttack.InitializeObjectPool(littleBlob, maxLittleBlobsCount);
        enemy.enemyAttack.SpawnObjectOverTime(minLittleBlobsSpawnRate, maxLittleBlobsSpawnRate);
    }
    private void Die(Enemy enemy)
    {
        if (this.enemy == enemy)
        {
            enemy.enemyAttack.SpawnObject();
            enemy.enemyAttack.SpawnObject();
        }
    }

}
