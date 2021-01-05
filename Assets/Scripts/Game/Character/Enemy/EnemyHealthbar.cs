using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Enemy))]
public class EnemyHealthbar : MonoBehaviour
{
    [SerializeField] SpriteRenderer frontLine;
    [SerializeField] TextMeshPro healthText;
    [SerializeField] TakeDamage target;
    CharacterStats enemyStats;
    float multiplier;

    void Start()
    {
        Enemy enemy = GetComponentInParent<Enemy>();
        //target.OnDamageRecieved.AddListener(DamageRecievedHandler);
        enemyStats = enemy.CharacterStats;
        multiplier = 1f / enemyStats.MaxHealth;
        SetHealthValue(enemyStats.Health, enemyStats.MaxHealth);

        enemy.CharacterStats.OnCharacterPropertyChanged.AddListener((a, b) =>
        {
            if (a == CharacterStats.PropertyID.HEALTH || a == CharacterStats.PropertyID.MAXHEALTH)
            {
                SetHealthValue(enemyStats.Health, enemyStats.MaxHealth);
                Debug.Log("Health or MaxHealth changed");
            } 
        });
    }
    void DamageRecievedHandler(Bullet.DamageInfo damageInfo, Transform target)
    {
        if (enemyStats.Health > 0)
        {
            SetHealthValue(enemyStats.Health, enemyStats.MaxHealth);
        }
        else
        {
            SetHealthValue(0, enemyStats.MaxHealth);
        }
    }
    void SetHealthValue(float health, float maxHealth)
    {

        frontLine.transform.localScale = new Vector3(health * multiplier, 1, 1);
        healthText.text = health + "/" + maxHealth;
    }
}
