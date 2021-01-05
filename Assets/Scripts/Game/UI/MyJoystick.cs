using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
public class MyJoystick : MonoBehaviour
{
    [System.Serializable]
    enum Destination
    {
        MOVE, 
        LOOK
    }
    static bool[] activeTouches = new bool[2];
    [SerializeField] GameObject Stick;
    [SerializeField] Destination destination = Destination.MOVE;
    RectTransform stickRect;

    Vector2 startPosition;
    PlayerController playerController;

    Finger myFinger;
    Vector2 xSize;
    Vector2 ySize;

    public float movementRange
    {
        get => m_MovementRange;
        set => m_MovementRange = value;
    }

    [SerializeField]
    private float m_MovementRange = 50;

    private void Awake()
    {
        stickRect = Stick.GetComponent<RectTransform>();
        startPosition = stickRect.position;
        EnhancedTouchSupport.Enable();
        RectTransform rectTransform = GetComponent<RectTransform>();
        float index = Screen.height / 1080f;
        
        xSize.x = rectTransform.position.x - rectTransform.sizeDelta.x * index / 2;
        xSize.y = rectTransform.position.x + rectTransform.sizeDelta.x * index / 2;
        ySize.x = rectTransform.position.y - rectTransform.sizeDelta.y * index / 2;
        ySize.y = rectTransform.position.y + rectTransform.sizeDelta.y * index / 2;
        movementRange *= index;
        //Debug.Log("Pos: " + rectTransform.position.x + " " + rectTransform.position.y);
        //Debug.Log("Screen: " + Screen.width + " " + Camera.main.pixelHeight);
        //Debug.Log("RectSize: " + rectTransform.rect.width + " " + rectTransform.rect.height);
        //Debug.Log("SizeDelta: " + rectTransform.sizeDelta.x * index + " " + rectTransform.sizeDelta.y * index);
        //Debug.Log("Size: " + xSize.x + " " + xSize.y + "     :     " + ySize.x + " " + ySize.y);
    }
    private void Start()
    {
        if (!Player.Instance) return;
        playerController = Player.Instance.PlayerController;
        Subscribe(true);
    }
    private void OnEnable()
    {
        if (!Player.Instance) return;
        playerController = Player.Instance.PlayerController;
        Subscribe(true);
    }
    private void OnDisable()
    {
        Subscribe(false);
        myFinger = null;
        Move(startPosition);
    }
    private void OnDestroy()
    {
        Subscribe(false);
        EnhancedTouchSupport.Disable();
    }
    bool subscribed = false;
    void Subscribe(bool subscribe)
    {
        if (!subscribed && subscribe)
        {
            subscribed = true;
            Touch.onFingerDown += OnPointerDown;
            Touch.onFingerMove += OnPointerMove;
            Touch.onFingerUp += OnPointerUp;
        }
        else if (subscribed && !subscribe)
        {
            subscribed = false;
            Touch.onFingerDown -= OnPointerDown;
            Touch.onFingerMove -= OnPointerMove;
            Touch.onFingerUp -= OnPointerUp;
        }
    }
    public void OnPointerDown(Finger finger)
    {
        if (myFinger != null) return;
        Vector2 position = finger.screenPosition;
        if (position.x > xSize.x && position.x < xSize.y && position.y > ySize.x && position.y < ySize.y)
        {
            myFinger = finger;

            Move(position);
        }
    }

    void Move(Vector2 value)
    {
        //Debug.Log(value.x + " " + value.y);
        value = value - startPosition;
        value = Vector2.ClampMagnitude(value, m_MovementRange);
        stickRect.position = value + startPosition;
        value = value.normalized;
        if (destination == Destination.MOVE)
            playerController.Move(value);
        else
            playerController.Look(value);
    }
    public void OnPointerMove(Finger finger)
    {
        if (myFinger == null) return;
        if (finger != myFinger) return;
        Move(finger.screenPosition);
    }

    public void OnPointerUp(Finger finger)
    {
        if (finger != myFinger) return;
        
        Move(startPosition);
        myFinger = null;
        
    }
}
