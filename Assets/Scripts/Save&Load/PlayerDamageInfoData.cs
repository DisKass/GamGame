using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageInfoData : IPersistentData<Player>
{
    public Bullet.DamageInfo damageInfo;
    public Dictionary<Bullet.DamageTypeValues, uint> damageTypeCount;
    public void Initialize(Player persistendObject)
    {
        damageTypeCount = persistendObject.GetDamageTypeCount();
        damageInfo = persistendObject.weaponManager.bulletInfo.DamageInfo;
    }
}
