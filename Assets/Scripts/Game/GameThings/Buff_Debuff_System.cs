using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

//[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(ICharacter))]
public class Buff_Debuff_System : MonoBehaviour
{
    public struct CurrentBuffs
    {
        public bool Burning;
        public bool Freezing;
        public bool Stasis;
        public bool Immune;
        public void Reset()
        {
            Burning = Freezing = Stasis = Immune = false;
        }
    }
    [HideInInspector] public CurrentBuffs currentBuffs;
    CharacterStats characterStats;
    TakeDamage takeDamage;
    Bullet.DamageInfo damageInfo;
    Material material;
    [SerializeField] Material defaultMaterial;
    FollowingCamera mainCamera;
    private int buffsCount = 0;
    private int stunCount = 0;
    ICharacter character;
    bool IsActive = false;
    public void Initialize()
    {
        character = GetComponent<ICharacter>();
        mainCamera = Camera.main.GetComponent<FollowingCamera>();
        if (!TryGetComponent(out character))
        {
            enabled = false;
            return;
        }
        characterStats = character.CharacterStats;
        takeDamage = character.TakeDamage;
        OnCharacterStunned.AddListener(character.Stun);
        IsActive = true;
        //takeDamage.OnDamageRecieved.AddListener(HandleDamageRecieved);
        //material = GetComponent<SpriteRenderer>().material;
        //material = GetComponent<SpriteRenderer>().material;
    }

    //void HandleDamageRecieved(Bullet.DamageInfo damageInfo, Transform target)
    //{
    //    //if (damageInfo.damageType.IsFire) Burn(damageInfo.Damage / 10);
    //    if (damageInfo.damageType.IsIce) Freeze(WeaponManager.Instance.bulletInfo.DamageInfo.Damage);
    //}

    void SetDefaultMaterial()
    {
        GetComponent<SpriteRenderer>().material = defaultMaterial;
        SetPostProcessing(false);
    }
    void SetMaterialFloat(string flag, float value)
    {
        GetComponent<SpriteRenderer>().material.SetFloat(flag, value);
        SetPostProcessing(value == 1);
    }
    void SetPostProcessing(bool value)
    {
        mainCamera.setPostProcessing(true); // value
    }
    void StunCharacter(bool value)
    {
        if (value && stunCount == 0)
        {
            OnCharacterStunned.Invoke(true);
        }
        if (!value && stunCount == 1)
        {
            OnCharacterStunned.Invoke(false);
        }
        stunCount += value ? 1 : -1;
    }

    #region Burning
    Coroutine BurnCoroutine;
    public void Burn(int Damage)
    {
        if (!IsActive) return;
        if (characterStats.IsDead) return;
        BurnCoroutine = BeginBurning(Mathf.RoundToInt(Damage*0.2f));
    }
    Coroutine BeginBurning(int Damage)
    {
        damageInfo.UpdateInfo(Damage: Damage, new Bullet.DamageType { IsFire = true, IsDot = true });
        if (!currentBuffs.Burning)
        {
            currentBuffs.Burning = true;
            buffsCount++;
            SetMaterialFloat("_IsBurning", 1);
        }
        else
        {
            StopCoroutine(BurnCoroutine);
        }
        return StartCoroutine(Burn(damageInfo, 0.2f, 2));
    }
    void EndBurning()
    {
        currentBuffs.Burning = false;
        buffsCount--;
        if (buffsCount == 0)
            SetDefaultMaterial();
        else
            SetMaterialFloat("_IsBurning", 0);
    }
    IEnumerator Burn(Bullet.DamageInfo Damage, float rate, float time)
    {
        float startTime = Time.time;
        while (startTime + time > Time.time)
        {
            takeDamage.Damage(Damage, null);
            yield return new WaitForSeconds(rate);
        }
        EndBurning();
    }
    #endregion

    #region Freezing
    Coroutine FreezeCoroutine;

    public Events.EventCharacterStunned OnCharacterStunned = new Events.EventCharacterStunned();
    public void Freeze(int Damage)
    {
        if (!IsActive) return;
        if (characterStats.IsDead) return;
        if (currentBuffs.Freezing)
        {
            StopCoroutine(FreezeCoroutine);
            buffsCount--;
        }
        FreezeCoroutine = StartCoroutine(Freeze(Damage*0.1f));
    }
    IEnumerator Freeze(float time)
    {
        buffsCount++;
        StunCharacter(true);
        currentBuffs.Freezing = true;
        SetMaterialFloat("_IsFreezing", 1);
        yield return new WaitForSeconds(time);

        currentBuffs.Freezing = false;
        StunCharacter(false);
        if (--buffsCount == 0)
            SetDefaultMaterial();
        else
            SetMaterialFloat("_IsFreezing", 0);
    }
    #endregion

    #region Stasis
    Coroutine StasisCoroutine;
    public void BreakStasis()
    {
        if (currentBuffs.Stasis)
        {
            StopCoroutine(StasisCoroutine);

            currentBuffs.Stasis = false;
            if (--buffsCount == 0)
                SetDefaultMaterial();
            else
                SetMaterialFloat("_IsStasis", 0);
        }
    }
    public void ActivateStasis(float time)
    {
        if (!IsActive) return;
        if (characterStats.IsDead) return;
        if (currentBuffs.Stasis)
        {
            StopCoroutine(StasisCoroutine);
            buffsCount--;
        }
        StasisCoroutine = StartCoroutine(Stasis(time));
    }
    IEnumerator Stasis(float time)
    {
        StunCharacter(true);
        Debug.Log("Stasis");
        buffsCount++;
        currentBuffs.Stasis = true;
        SetMaterialFloat("_IsStasis", 1);
        yield return new WaitForSeconds(time);

        StunCharacter(false);
        currentBuffs.Stasis = false;
        if (--buffsCount == 0)
            SetDefaultMaterial();
        else
            SetMaterialFloat("_IsStasis", 0);
    }
    #endregion

    #region Immune
    public bool IsImmune { get => currentBuffs.Immune; set => currentBuffs.Immune = value; }
    #endregion


    public void Die()
    {
        StopAllCoroutines();
        OnCharacterStunned.RemoveListener(character.Stun);
        currentBuffs.Reset();
        for (int i = 0; i < buffsCount; i++)
        {
            SetPostProcessing(false);
        }
        IsActive = false;
    }
    //private void OnDisable()
    //{
    //    StopAllCoroutines();
    //}
}
