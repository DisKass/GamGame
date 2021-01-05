using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormSpear : Boss
{
    [SerializeField] StasisCircle stasisCircle;
    bool IsAttacking = true;
    bool IsMoving = true;
    Transform playerTransform;
    new void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        playerTransform = Player.Instance.Transform;
        GameObject container = new GameObject();
        stasisCircle = Instantiate(stasisCircle, container.transform);
        stasisCircle.OnHitPlayer.AddListener(Dash);
        container.transform.localScale = new Vector2(1.2f, 1.2f);
        StartCoroutine(Attacking());
        //StartCoroutine(Moving());
    }

    IEnumerator Attacking()
    {
        Vector2 direction;
        
        while (IsAttacking)
        {
            stasisCircle.Activate(1.5f, Player.Instance.Transform.position);
            yield return new WaitForSeconds(1f);// + 0.2f * CharacterStats.Health / CharacterStats.MaxHealth);
            direction = playerTransform.position - Transform.position;
            direction = direction.normalized;
            float angle = Vector2.SignedAngle(Vector2.right, direction);
            angle = Mathf.Deg2Rad * angle;
            direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            enemyAttack.MultyDirectionalFire(direction);
            yield return new WaitForSeconds(1.2f);// - 0.2f * CharacterStats.Health / CharacterStats.MaxHealth);
        }
    }
    IEnumerator Moving()
    {
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
    void Dash()
    {
        Vector2 direction = playerTransform.position - Transform.position;
        enemyAttack.Jump(direction, Rigidbody2D.mass * Rigidbody2D.drag * direction.magnitude * 1.3f);
    }
    public Vector2 RotateVector(Vector2 v, float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        float _x = v.x * Mathf.Cos(radian) - v.y * Mathf.Sin(radian);
        float _y = v.x * Mathf.Sin(radian) + v.y * Mathf.Cos(radian);
        return new Vector2(_x, _y);
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
