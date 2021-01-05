using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : LevelStateController
{
    [SerializeField] Portal entrancePortal;
    [SerializeField] Vector3 playerStartPosition;
    [SerializeField] string[] levels;
    int nextLevelIndex = 0;
    private void Start()
    {
        entrancePortal.OpenPortal(true);
        entrancePortal.IsActive = true;
        Enemy.CanSpawn = true;
        if (SaveSystem<Town, TownData>.CheckSaveFile("TownData"))
        {
            nextLevelIndex = SaveSystem<Town, TownData>.Load("TownData").nextLevel;
        }
        entrancePortal.SetTargetLevel(levels[nextLevelIndex]);
        ChangeState(LevelState.ENDING);
        Player.Instance.transform.position = playerStartPosition;
        Portal.OnTeleportPlayer.AddListener(ChangeNextLevel);
    }

    private void ChangeNextLevel(bool changingStarted)
    {
        if (changingStarted)
        {
            if (nextLevelIndex < levels.Length - 1)
            {
                nextLevelIndex++;
                SaveSystem<Town, TownData>.Save(this, "TownData");
            }
        }
    }
    public int GetNextLevelIndex()
    {
        return nextLevelIndex;
    }
}
