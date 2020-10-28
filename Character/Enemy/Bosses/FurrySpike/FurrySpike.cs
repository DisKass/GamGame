using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurrySpike : Boss
{
    Vector2[] directions = new Vector2[16]
    {
        Vector2.up.normalized,
        Vector2.down.normalized,
        Vector2.left.normalized,
        Vector2.right.normalized,
        (Vector2.up + Vector2.left).normalized,
        (Vector2.up + Vector2.right).normalized,
        (Vector2.down + Vector2.left).normalized,
        (Vector2.down + Vector2.right).normalized,
        (Vector2.up*2 + Vector2.left).normalized,
        (Vector2.up*2 + Vector2.right).normalized,
        (Vector2.down*2 + Vector2.left).normalized,
        (Vector2.down*2 + Vector2.right).normalized,
        (Vector2.up + Vector2.left*2).normalized,
        (Vector2.up + Vector2.right*2).normalized,
        (Vector2.down + Vector2.left*2).normalized,
        (Vector2.down + Vector2.right*2).normalized
    };
    [SerializeField] float waitTime;
    bool IsAttacking = true;
    bool IsMoving = true;
    new void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(Attacking());
        StartCoroutine(Moving());
    }

    IEnumerator Attacking()
    {
        while (IsAttacking)
        {
            //float angle = Random.value * 360;
            //for (int i = 0; i < directions.Length; i++)
            //{
            //    directions[i] = RotateVector(directions[i], angle);
            //}
            enemyAttack.MultyDirectionalFire(directions);
            yield return new WaitForSeconds(waitTime);
        }
    }
    IEnumerator Moving()
    {
        Transform playerTransform = Player.Instance.Transform;
        while (IsMoving && playerTransform != null)
        {
            //float angle = Random.value * 360;
            //for (int i = 0; i < directions.Length; i++)
            //{
            //    directions[i] = RotateVector(directions[i], angle);
            //}

            enemyMove.MoveTo(playerTransform.position - Transform.position, CharacterStats.Speed);
            yield return new WaitForEndOfFrame();
        }
    }
    
    public Vector2 RotateVector(Vector2 v, float angle)
    {
        float radian = angle*Mathf.Deg2Rad;
        float _x = v.x*Mathf.Cos(radian) - v.y*Mathf.Sin(radian);
        float _y = v.x*Mathf.Sin(radian) + v.y*Mathf.Cos(radian);
        return new Vector2(_x,_y);
    }
    override public void Die()
    {
        if (CharacterStats.IsDead) return;
        base.Die();
        Dissolve dissolve = EnemyDeathEffect.GetComponent<Dissolve>();

        dissolve.SetSpeed(0.005f);
        dissolve.SetScale(3.7f);
    }
}
