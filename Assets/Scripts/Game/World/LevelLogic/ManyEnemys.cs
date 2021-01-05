using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Game.Items;

public class ManyEnemys : LevelStateController
{
    [SerializeField] bool IsEnemysSpawnInActionState = false;
    [SerializeField] Portal entrancePortal;
    int enemyCount;
    [SerializeField] ItemPlatform ItemPlatform;
    [HideInInspector]
    public int itemID;
    Transform lastEnemyTransform;
    protected override void Awake()
    {
        base.Awake();
        Enemy.OnEnemySpawned.AddListener(AddEnemy);
        Enemy.OnEnemyDie.AddListener(RemoveEnemy);
        Player.Instance.OnPlayerLeftSafeZone.AddListener(HandlePlayerLeftSafeZone);
        GameManager.Instance.OnGameStateChanged.AddListener((a, b) =>
        {
            if (a == GameManager.GameState.CHANGELEVEL) Reset();
        });
        GameManager.Instance.OnStoreData.AddListener((a, b) =>
        {
            if (b == "LevelStateController")
                if (a == GameManager.StoreDataType.SAVE) Save();
            //if (a == GameManager.StoreDataType.RESET) Reset();
        });
        if (!Load())
            ChangeState(LevelState.BEGINNING);
    }
    void HandlePlayerLeftSafeZone(bool safe)
    {
        if (!safe && CurrentLevelState == LevelState.BEGINNING) ChangeState(LevelState.ACTION); 
    }

    public float[] GetItemPlatformPosition()
    {
        if (ItemPlatform != null)
            return new float[2] { ItemPlatform.transform.position.x, ItemPlatform.transform.position.y };
        else return null;
    }

    public override void ChangeState(LevelState levelState)
    {
        Debug.Log(levelState);
        switch (levelState)
        {
            case LevelState.BEGINNING:
                Enemy.CanSpawn = true;
                entrancePortal.IsActive = false;
                entrancePortal.OpenPortal(true);
                if (entrancePortal.IsEnterPortal) entrancePortal.MovePlayerToPortal();
                break;
            case LevelState.ACTION:
                entrancePortal.OpenPortal(false);
                break;
            case LevelState.ENDING:
                Enemy.CanSpawn = false;
                if (IsLastLevel)
                {
                    GameManager.Instance.EndOfGame();
                    return;
                }
                entrancePortal.OpenPortal(true);
                entrancePortal.IsActive = true;
                if (ItemPlatform != null)
                {
                    if (itemID >= 1)
                    {
                        //Debug.Log("NotRandomItem!" + itemID);
                        ItemPlatform.SetItem(itemID);
                    }
                    else
                    {
                        //Debug.Log("RandomItem!");
                        itemID = ItemPlatform.SetItem(ItemPools.Instance.GetRandomItemToInstantiate());
                    }
                    //if (lastEnemyTransform != null)
                    //    ItemPlatform.transform.position = lastEnemyTransform.position;
                    ItemPlatform.gameObject.SetActive(true);
                }
                break;
            default:
                break;
        }
        base.ChangeState(levelState);
        if (CurrentLevelState == LevelState.BEGINNING || CurrentLevelState == LevelState.ENDING)
        {
            GameManager.Instance.SaveGame("LevelStateController");
        }

        //if (levelState == LevelState.ACTION && enemyCount == 0 && IsEnemysSpawnInActionState == false)
        //{
        //    ChangeState(LevelState.ENDING);
        //}
    }
    void Save()
    {
        if (CurrentLevelState == LevelState.ACTION) { return; }
        SaveSystem<ManyEnemys, ManyEnemysData>.Save(this, "LevelState");
    }

    bool Load()
    {
        itemID = -1;
        if (SaveSystem<ManyEnemys, ManyEnemysData>.CheckSaveFile("LevelState"))
        {
            ManyEnemysData data = SaveSystem<ManyEnemys, ManyEnemysData>.Load("LevelState");
            if (data.currentLevelState == LevelState.ENDING) itemID = data.itemID;
            if (data.ItemPlatformPosition != null)
            {
                //Vector2 ItemPostion = new Vector2(data.ItemPlatformPosition[0], data.ItemPlatformPosition[1]);
                //ItemPlatform.transform.position = ItemPostion;
            }
            ChangeState(data.currentLevelState);
            //Debug.Log("[ManyEnemys] Load success. ItemPlatform position: " + ItemPlatform.transform.position.x + " " + ItemPlatform.transform.position.y);
            return true;
        }
        //Debug.Log("[ManyEnemys] Load unsuccess.");
        return false;
    }
    void Reset()
    {
        Enemy.CanSpawn = true;
        SaveSystem<ManyEnemys, ManyEnemysData>.Delete("LevelState");
        Debug.Log("[ManyEnemys] Reset.");
    }

    List<Enemy> enemyPool = new List<Enemy>();
    public void AddEnemy(Enemy enemy)
    {
        enemyCount++;
        enemyPool.Add(enemy);
        //Debug.Log("Enemy count: " + enemyCount);
    }
    public void RemoveEnemy(Enemy enemy)
    {
        enemyCount--;
        //Debug.Log("Enemy count: " + enemyCount);
        if (enemyCount == 0)
        {
            lastEnemyTransform = enemy.transform;
            ChangeState(LevelState.ENDING);
        }
        enemyPool.Remove(enemy);
    }

}
