using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemys : MonoBehaviour
{
    [SerializeField] int minLvl;
    [SerializeField] int maxLvl;
    [SerializeField] Enemy enemy;
    [SerializeField] IllusiveEnemySpawner IllusiveEnemySpawnerPrefab;
    int chosenEnemyLvl;

    void Start()
    {
        chosenEnemyLvl = Random.Range(minLvl, maxLvl + 1);
        GameObject enemyToInstantiate = enemy?.gameObject ?? EnemySpawnManager.Instance.GetEnemy(chosenEnemyLvl);
        if (enemyToInstantiate == null)
        {
            Debug.LogWarning("[SpawnEnemys] Enemys With Lvl " + chosenEnemyLvl + " not found");
            return;
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).tag == "Spawnpoint")
            {
                GameObject instantiatedEnemy;
                instantiatedEnemy = Instantiate(enemyToInstantiate);
                instantiatedEnemy.transform.parent = transform.GetChild(i);
                instantiatedEnemy.transform.localPosition = Vector3.zero;
                instantiatedEnemy.SetActive(false);
                Instantiate(IllusiveEnemySpawnerPrefab, transform.GetChild(i).position, Quaternion.identity).GetComponent<IllusiveEnemySpawner>().Enemy = instantiatedEnemy.GetComponent<Enemy>();
            }
        }  
    }
}
