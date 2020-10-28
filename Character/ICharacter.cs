using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    CharacterStats CharacterStats { get; set; }
    Rigidbody2D Rigidbody2D { get; set; }
    TakeDamage TakeDamage { get; set; }
    SpriteRenderer SpriteRenderer { get; set; }
    Animator Animator { get; set; }
    Buff_Debuff_System Buffs { get; set; }
    Transform Transform { get; }

    void Stun(bool value);
    void Die();
}
