using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ManyEnemysData : IPersistentData<ManyEnemys>
{
    public int itemID;
    public LevelStateController.LevelState currentLevelState;
    public float[] ItemPlatformPosition = new float[2]; 
    public void Initialize(ManyEnemys persistendObject)
    {
        currentLevelState = persistendObject.CurrentLevelState;
        if (currentLevelState == LevelStateController.LevelState.ENDING)
        {
            itemID = persistendObject.itemID;
            ItemPlatformPosition = persistendObject.GetItemPlatformPosition();
        }
    }
}
