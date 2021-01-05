using UnityEngine.Events;
public class Events
{
    [System.Serializable] public class EventPosition : UnityEvent<UnityEngine.Vector3> { }
    [System.Serializable] public class EventDamageRecieved : UnityEvent<Bullet.DamageInfo, UnityEngine.Transform, UnityEngine.GameObject> { }
    [System.Serializable] public class EventFadeComplete : UnityEvent<bool> { }
    [System.Serializable] public class EventGameStateChanged : UnityEvent<GameManager.GameState, GameManager.GameState> { }
    [System.Serializable] public class EventDamageDone : UnityEvent<Bullet.BulletInfo, UnityEngine.GameObject> { }
    [System.Serializable] public class EventCharacterPropertyChanged : UnityEvent<CharacterStats.PropertyID, object> { }
    [System.Serializable] public class EventItemPickUpped : UnityEvent<CharacterStats.PropertyID, object> { }
    [System.Serializable] public class EventLanguageChanged : UnityEvent { }
    [System.Serializable] public class EventLevelLoaded : UnityEvent<string> { }
    [System.Serializable] public class EventDamageTypeChanged : UnityEvent<Bullet.DamageTypeValues, bool> { }
    [System.Serializable] public class EventDamageInfoChanged : UnityEvent<Bullet.DamageInfo> { }
    [System.Serializable] public class EventLevelStateChanged : UnityEvent<LevelStateController.LevelState> { }
    [System.Serializable] public class EventEnemySpawned : UnityEvent<Enemy> { }
    [System.Serializable] public class EventEnemyDespawned : UnityEvent<Enemy> { }
    [System.Serializable] public class EventPlayerLeftSafeZone : UnityEvent<bool> { }
    [System.Serializable] public class EventCharacterStunned : UnityEvent<bool> { }
    [System.Serializable] public class EventStoreData : UnityEvent<GameManager.StoreDataType, string> { }
    [System.Serializable] public class EventWeaponRotated : UnityEvent<UnityEngine.Vector3> { }
    [System.Serializable] public class EventMovePlayer : UnityEvent<bool> { }
    [System.Serializable] public class EventTeleportPlayer : UnityEvent<bool> { }
    [System.Serializable] public class EventPlayerFired : UnityEvent { }
    [System.Serializable] public class EventHitPlayer : UnityEvent { }
    [System.Serializable] public class EventPlayerStartsFire : UnityEvent {}
    [System.Serializable] public class EventPlayerLook : UnityEvent<UnityEngine.Vector2> {}
}