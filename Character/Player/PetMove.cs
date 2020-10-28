using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PetMove : MonoBehaviour
{
    [SerializeField] MoveType moveType;
    [SerializeField] float MaxSpeed;
    Player player;
    Rigidbody2D _rigidbody2D;
    Transform _transform;
    public bool paused = false;
    public bool chasePaused = false;
    public bool moveInDirectionPaused = false;

    Vector2 direction;

    public void Initialize()
    {
        player = Player.Instance;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
        StartMoving();
    }

    public void StartMoving()
    {
        paused = false;
        if (moveType.Chase)
        {
            chasePaused = false;
            StartCoroutine(Chase(player.characterInventory.transform));
        }
        if (moveType.MoveInFireDirection)
        {
            moveInDirectionPaused = true;
            player.PlayerController.OnPlayerLook.AddListener(ChangeDirection);
            StartCoroutine(MoveInDirection());
        }
    }
    public void ChangeDirection(Vector2 direction)
    {
        this.direction = direction;
        SetFlagsByDirection(direction);
    }
    void SetFlagsByDirection(Vector2 direction)
    {
        moveInDirectionPaused = direction.x == 0 && direction.y == 0 ||
            !ObjectInRange(player.characterInventory._transform, _transform, player.CharacterStats.BulletRange);
        chasePaused = !moveInDirectionPaused;
    }
    IEnumerator MoveInDirection()
    {
        while (enabled)
        {
            if (paused || moveInDirectionPaused)
                yield return new WaitWhile(() => paused || moveInDirectionPaused);
            _rigidbody2D.AddForce(Time.fixedDeltaTime * 60 * _rigidbody2D.drag * MaxSpeed * direction);
            if (ObjectInRange(player.characterInventory._transform, _transform, player.CharacterStats.BulletRange) == false)
            {
                chasePaused = false;
                yield return new WaitWhile(() => !ObjectInRange(player.characterInventory._transform, _transform, player.CharacterStats.BulletRange));
                SetFlagsByDirection(direction);
            }
            yield return new WaitForFixedUpdate();
        }
    }
    bool ObjectInRange(Transform startingTransform, Transform targetTransform, float range)
    {
        return (targetTransform.position - startingTransform.position).magnitude <= player.CharacterStats.BulletRange;
    }
    public bool IsPaused { set => paused = value; }
    public void StopMoving()
    {
        StopAllCoroutines();
    }
    IEnumerator Chase(Transform target)
    {
        Vector2 way;
        while (enabled && target)
        {
            if (paused || chasePaused)
            {
                yield return new WaitWhile(() => paused || chasePaused);
            }
            way = target.position - _transform.position;
            way = Vector2.ClampMagnitude(way, MaxSpeed);
            _rigidbody2D.AddForce(Time.fixedDeltaTime * 60 * _rigidbody2D.drag * way);
            yield return new WaitForFixedUpdate();
        }
    }

    void OnDestroy()
    {
        player?.PlayerController.OnPlayerLook.RemoveListener(ChangeDirection);
    }
}
