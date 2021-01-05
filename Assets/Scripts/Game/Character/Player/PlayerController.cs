using UnityEngine;

[RequireComponent(typeof(Player))]

public class PlayerController : MonoBehaviour
{
    CharacterStats playerStats;
    WeaponManager weaponManager;
    SpriteRenderer spriteRenderer;
    PlayerAnimationController animationController;

    float playerSpeed;
    float playerDrag;


    bool isFire = false;
    bool CanFire = true;

    private Vector2 _movement = Vector2.zero;
    private Rigidbody2D _rigidbody;
    private Transform _transform;

    private int _currentYPos;

    bool IsFacingRight = true;

    bool IsMoving = false;
    bool CanMove = true;
    bool CanLook = true;
    public bool IsStunned
    {
        set
        {
            if (value)
                animationController.Pause();
            else
                animationController.Continue();
            CanMove = !value;
            CanLook = !value;
            CanFire = !value;
        }
    }

    public Events.EventPlayerStartsFire OnPlayerStartsFire = new Events.EventPlayerStartsFire();
    public Events.EventPlayerLook OnPlayerLook = new Events.EventPlayerLook();
    Player player;
    
    public void Initialize()
    {
        player = GetComponent<Player>();
        
        playerStats = player.CharacterStats;
        playerSpeed = playerStats.Speed;
        playerDrag = playerStats.Drag;
        playerStats.OnCharacterPropertyChanged.AddListener(HandleCharacterPropertyChanged);
        weaponManager = player.weaponManager;
        _rigidbody = player.Rigidbody2D;
        _rigidbody.drag = playerDrag;
        spriteRenderer = player.SpriteRenderer;
        //Debug.Log("[PlayerController] SpriteRenderer Is null: " + (spriteRenderer == null));
        animationController = player.PlayerAnimationController;

        _transform = player.Transform;
        //Debug.Log("[PlayerController] Transform Is null: " + (Transform == null));
        //_transform.position = playerStats.Position;
        //StartCoroutine("LateStart");
    }

    private void HandleCharacterPropertyChanged(CharacterStats.PropertyID property, object value)
    {
        if (property == CharacterStats.PropertyID.SPEED || property == CharacterStats.PropertyID.SPEEDMULTIPLIER)
        {
            playerSpeed = playerStats.Speed;
        }
        if (property == CharacterStats.PropertyID.DRAG)
        {
            playerDrag = playerStats.Drag;
            _rigidbody.drag = playerDrag;
        }
    }

    void Update()
    {
        if (isFire&&CanFire) weaponManager.Fire();
        if (CanMove && IsMoving) 
            _rigidbody.AddForce(player.Rigidbody2D.mass * playerDrag * playerSpeed * Time.deltaTime * 60 * _movement, ForceMode2D.Force); // 60 = fps

    }
    public void Move(Vector2 movement)
    {
        _movement = movement;
        IsMoving = _movement.x != 0 || _movement.y != 0;
        animationController.Move(IsMoving);
        if (!isFire) GamepadLook(movement); // isFire == true только когда пользователь касается второго джойстика.
    }


    public void Look(Vector2 values)
    {
        if (!CanLook) return;
        if (!isFire && values.magnitude != 0) OnPlayerStartsFire.Invoke();
        isFire = values.magnitude != 0;

        if (values == Vector2.zero)
            return;

        if (values.x < -1 || values.x > 1)
        values = values - (Vector2)Camera.main.WorldToScreenPoint(WeaponManager.Instance._currentWeapon.transform.position);
        GamepadLook(values);
        OnPlayerLook.Invoke(values);
    }

    private void GamepadLook(Vector2 value)
    {
        if (value.x != 0)
        {
            if (IsFacingRight && value.x < 0) { Flip(); IsFacingRight = false; }
            if (!IsFacingRight && value.x > 0) { Flip(); IsFacingRight = true; }
            //ShowInformation.Instance.AddMessage(signX.ToString() + " " + Mathf.Sign(value.x));
            //_transform.localScale = new Vector3(signX, 1, 1);
            WeaponManager.Instance.sortingOrder = spriteRenderer.sortingOrder - (int)_transform.localScale.x;
            //WeaponManager.Instance.SetRotation(Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, value.normalized)), IsFacingRight);
        }

    }
    void Flip()
    {
        _transform.Rotate(0f, 180f, 0f);
        CharacterInventory.Instance.transform.Rotate(0f, 180f, 0f);
    }
    private void MouseLook(Vector2 value)
    {
        //if (xCoord > screenWidth / 2)
        //{
        //    _transform.localScale = new Vector3(1, 1, 1);
        //    WeaponManager.Instance.sortingOrder = _playerSpriteRenderer.sortingOrder - 1;
        //}
        //if (xCoord < screenWidth / 2)
        //{
        //    _transform.localScale = new Vector3(-1, 1, 1);
        //    WeaponManager.Instance.sortingOrder = _playerSpriteRenderer.sortingOrder + 1;
        //}
    }
    private void OnDestroy()
    {
        playerStats?.OnCharacterPropertyChanged?.RemoveListener(HandleCharacterPropertyChanged);
    }
}
