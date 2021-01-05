using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] MainMenu _mainMenu;
    [SerializeField] ItemMenu _itemMenu;
    [SerializeField] EndGameMenu _endGameMenu;

    [SerializeField] OnPlayUI _onPlayUI;
    [SerializeField] Camera _dummyCamera;
    [SerializeField] LoadingScreen _loadingScreen;
    [SerializeField] public GameObject BossHealthbar;
    public InputAction MenuClickAction;
    public InputAction CancleAction;

    public Events.EventFadeComplete OnMainMenuFadeComplete;
    public Events.EventFadeComplete OnLoadingScreenFadeComplete;
    private void Start()
    {
        _mainMenu.OnMainMenuFadeComplete.AddListener(HandleMainMenuFadeComplete);
        _loadingScreen.OnLoadingScreenFadeComplete.AddListener(HandleLoadingScreenFadeComplete);
        MenuClickAction.performed += MainMenuFadeOut;
        CancleAction.performed += HandleCancelAction;
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanded);
        _itemMenu.Initialize();
        _endGameMenu.Initialize();
    }
    private void OnEnable()
    {
        MenuClickAction.Enable();
        CancleAction.Enable();
    }
    private void OnDisable()
    {
        MenuClickAction.Disable();
        CancleAction.Disable();
    }
    void HandleCancelAction(InputAction.CallbackContext ctx)
    {
        GameManager.Instance.TogglePause();
    }
    void HandleMainMenuFadeComplete(bool fadeOut)
    {
        OnMainMenuFadeComplete.Invoke(fadeOut);
        if (!fadeOut) MenuClickAction.Enable();
    }
    void HandleLoadingScreenFadeComplete(bool fadeOut)
    {
        OnLoadingScreenFadeComplete.Invoke(fadeOut);
    }
    void HandleGameStateChanded(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        _itemMenu.gameObject.SetActive(currentState == GameManager.GameState.SHOWINGITEMS);
        _endGameMenu.gameObject.SetActive(currentState == GameManager.GameState.ENDGAMEMENU);
        _onPlayUI.gameObject.SetActive(currentState == GameManager.GameState.RUNNING);
    }

    public void MainMenuFadeOut(InputAction.CallbackContext ctx)
    {
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.PREGAME)
        {
            return;
        }
        GameManager.Instance.StartGame();
        MenuClickAction.Disable();
    }

    public void SetDummyCameraActive(bool active)
    {
        _dummyCamera.gameObject.SetActive(active);
    }

    public void StartLoadingScreenFadeIn()
    {
        _loadingScreen.FadeIn();
    }
}
