using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TownData : IPersistentData<Town>
{
    public int nextLevel = 0;
    public void Initialize(Town persistendObject)
    {
        nextLevel = persistendObject.GetNextLevelIndex();
    }
}
