using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameManagerData: IPersistentData<GameManager>
{
    public string lastLocation;
    public GameManagerData() { }
    public GameManagerData(GameManager gameManager)
    {
        Initialize(gameManager);
    }
    public void Initialize(GameManager persistendObject)
    {
        lastLocation = persistendObject.CurrentLevelName;
        //Debug.Log("[GameManagerData] Stored Level Name" + lastLocation);
        //if (persistendObject.NotGameLevel.Contains(lastLocation)) return false;
        //return true;
    }
}
