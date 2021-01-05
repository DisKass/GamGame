using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Portal : MonoBehaviour
{
    [SerializeField] string levelToLoad;
    static string currentLevel;

    [SerializeField] public bool IsEnterPortal = false;
    [SerializeField] bool isActive = false;
    [SerializeField] bool IsAlwaysActive = false;
    Animator animator;
    Transform _transform;
    static public Events.EventMovePlayer OnMovePlayer = new Events.EventMovePlayer();
    static public Events.EventTeleportPlayer OnTeleportPlayer = new Events.EventTeleportPlayer();
    public bool IsActive { get => isActive; set => isActive = IsAlwaysActive || value; }

    //private void Awake()
    //{
    //    if (IsAlwaysActive) IsActive = true;
    //    gameObject.SetActive(IsActive);
    //}

    public void MovePlayerToPortal()
    {
        OnMovePlayer.Invoke(true);
        Player.Instance.transform.position = transform.position;
        OnMovePlayer.Invoke(false);
    }

    public void OpenPortal(bool open)
    {
        _transform = transform;
        gameObject.SetActive(true);
        if (IsAlwaysActive) open = true;
        StopAllCoroutines();
        StartCoroutine(fade(open ? 1 : 0));
    }

    IEnumerator fade(float targetValue)
    {
        Vector2 scale = _transform.localScale;
        float step = (targetValue - scale.x) / 10;
        int i = 0;
        while (i++ < 10)
        {
            scale.x += step;
            _transform.localScale = scale;
            yield return new WaitForSeconds(0.02f);
        }
        gameObject.SetActive(targetValue == 0 ? false : true);
    }

    public void SetTargetLevel(string targetLevel)
    {
        levelToLoad = targetLevel;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsActive) return;
        OnTeleportPlayer.Invoke(true);
        GameManager.Instance.ChangeLevel(levelToLoad);
        OpenPortal(false);
        IsActive = false;
    }
}
