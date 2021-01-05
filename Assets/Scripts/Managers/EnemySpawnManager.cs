using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : Singleton<EnemySpawnManager>
{
    [SerializeField] GameObject[] _enemysPrefabs;
    CharacterStats_SO[] _enemysStats;
    List<GameObject>[] _enemyByLvl;

    public Events.EventEnemySpawned OnEnemySpawned = new Events.EventEnemySpawned();
    int maxLvl;
    protected override void Awake()
    {
        base.Awake();
        _enemyByLvl = new List<GameObject>[10];
        _enemysStats = new CharacterStats_SO[_enemysPrefabs.Length];
        for (int i = 0; i < _enemyByLvl.Length; i++)
        {
            _enemyByLvl[i] = new List<GameObject>();
        }
        for (int i = 0; i < _enemysPrefabs.Length; i++)
        {
            _enemysStats[i] = _enemysPrefabs[i].GetComponent<CharacterStats>()._characterStats;
            _enemyByLvl[_enemysStats[i].level].Add(_enemysPrefabs[i].gameObject);
        }
    }

    public GameObject GetEnemy(int lvl)
    {
        int number = _enemyByLvl[lvl].Count;
        if (number > 0)
            return _enemyByLvl[lvl][Random.Range(0, number)];
        else return null;
    }

}
