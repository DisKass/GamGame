using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStats", menuName = "Character/Stats", order = 1)]
public class CharacterStats_SO : ScriptableObject
{
    #region Fields
    public int level = 1;
    public int maxHealth = 100;
    public float maxHealthMultiplier = 1f;
    public int healthRegeneration;
    public float healthRegenerationMultiplier = 1f;
    public int damage = 10;
    public float damageMultiplier = 1f;
    public float critChance = 0.2f;
    public float critMultiplier = 2f;
    public float speed = 6f;
    public float speedMultiplier = 1f;
    public float drag = 5f;
    public float reload = 1f;
    public float reloadMultiplier = 1f;
    public float bulletSpeed = 20f;
    public float bulletLifeTime = 1f;
    public int currentWeapon = 0;

    public float attackRange = 20f;

    public Sprite sprite;
    public RuntimeAnimatorController _playerAnimatorController;

    public Vector2 position = Vector2.zero;

    public bool IsDead = false;
    #endregion


    #region Stat Increasers
    #endregion
}
