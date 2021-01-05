using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Healthbar : MonoBehaviour
{
    [SerializeField] SpriteRenderer frontLine;
    [SerializeField] SpriteRenderer backLine;
    [SerializeField] MonoBehaviour targetICharacter;
    ICharacter target;
    CharacterStats characterStats;
    float multiplier;

    private void OnValidate()
    {
        if (!(targetICharacter is ICharacter))
            targetICharacter = null;
    }

    public void SetAlpha(float value)
    {
        Color color = frontLine.color;
        color.a = value;
        frontLine.color = color;
        color = backLine.color;
        color.a = value;
        backLine.color = color;
    }
    void Start()
    {
        target = targetICharacter as ICharacter;
        //target.TakeDamage.OnDamageRecieved.AddListener(DamageRecievedHandler);
        target.CharacterStats.OnCharacterPropertyChanged.AddListener((property, value) =>
        {
            if (property == CharacterStats.PropertyID.HEALTH)
            {
                SetHealthValue((int)value);
            }
            if (property == CharacterStats.PropertyID.MAXHEALTH)
            {
                multiplier = 1f / (int)value;
            }
        });
        characterStats = target.CharacterStats;
        multiplier = 1f / characterStats.MaxHealth;
        SetHealthValue(characterStats.Health);
    }
    void DamageRecievedHandler(Bullet.DamageInfo damageInfo, Transform target)
    {
        if (characterStats.Health > 0)
        {
            frontLine.transform.localScale = new Vector3(characterStats.Health * multiplier, 1, 1);
        }
        else
        {
            frontLine.transform.localScale = new Vector3(0, 1, 1);
        }
    }
    void SetHealthValue(int health)
    {
        frontLine.transform.localScale = new Vector3(health * multiplier, 1, 1);
    }
}
