using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(ICharacter))]
public class TakeDamage : MonoBehaviour
{
    [SerializeField] bool IsPlayer = false;

    CharacterStats _characterStats;
    ICharacter character;

    public Events.EventDamageRecieved OnCharacterDamageRecieved = new Events.EventDamageRecieved();
    static public Events.EventDamageRecieved OnAnyCharacterDamageRecieved = new Events.EventDamageRecieved();

    public void Initialize()
    {
        OnCharacterDamageRecieved.AddListener(Pop_Ups.Instance.HandleOnDamageRecieved);

        character = GetComponent<ICharacter>();
        _characterStats = character.CharacterStats;

    }
    public void Damage(Bullet.DamageInfo damageInfo, GameObject source)
    {
        Debug.Log(source.name);
        if (_characterStats == null) return;
        if (character.Buffs.IsImmune) return;
        if (Random.value < damageInfo.CritChance)
        {
            damageInfo.Damage = (int)(damageInfo.Damage * damageInfo.CritMultiplier);
            damageInfo.damageType.IsCritical = true;
        }
        _characterStats.Health -= damageInfo.Damage;
        if (_characterStats.Health <= 0)
        {
            character.Die();
        }
        if (damageInfo.damageType.IsFire && !damageInfo.damageType.IsDot) character.Buffs.Burn(damageInfo.Damage);
        if (damageInfo.damageType.IsIce) character.Buffs.Freeze(damageInfo.Damage);
        OnCharacterDamageRecieved.Invoke(damageInfo, transform, source);
        OnAnyCharacterDamageRecieved.Invoke(damageInfo, transform, source);
    }
    private void OnDisable()
    {
        OnCharacterDamageRecieved.RemoveAllListeners();
    }
    private void OnDestroy()
    {
        OnCharacterDamageRecieved.RemoveAllListeners();
    }
    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.TryGetComponent(out Bullet b))
    //    {
    //        Damage(b._damageInfo);
    //    }
    //}
}
