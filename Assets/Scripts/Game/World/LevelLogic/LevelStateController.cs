using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelStateController : Singleton<LevelStateController>
{
    [System.Serializable]
    public enum LevelState
    {
        BEGINNING,
        ACTION,
        ENDING
    }
    [SerializeField] protected bool IsLastLevel = false;
    public static Events.EventLevelStateChanged OnLevelStateChanged = new Events.EventLevelStateChanged();
    private LevelState currentLevelState;
    public LevelState CurrentLevelState { get => currentLevelState; set => currentLevelState = value; }
    protected override void Awake()
    {
        base.Awake();
    }

    virtual public void ChangeState(LevelState levelState)
    {
        CurrentLevelState = levelState;
        if (levelState == LevelState.ENDING)
        {
            Player.Instance?.RestoreHealth();
        }
        OnLevelStateChanged.Invoke(levelState);
    }
}
