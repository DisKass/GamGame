using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicHead : MonoBehaviour
{
    [SerializeField] GameObject bomb;
    Enemy enemy;
    int maxLittleBlobsCount = 3;
    float minLittleBlobsSpawnRate = 3;
    float maxLittleBlobsSpawnRate = 5;
    private void Start()
    {
        enemy = GetComponent<Enemy>();
        enemy.enemyAttack.InitializeObjectPool(bomb, maxLittleBlobsCount);
        enemy.enemyAttack.SpawnObjectOverTime(minLittleBlobsSpawnRate, maxLittleBlobsSpawnRate);
        enemy.enemyMove.Escape(Player.Instance.Transform, enemy.CharacterStats.Speed, 
            Player.Instance.CharacterStats.BulletRange);
    }
}
