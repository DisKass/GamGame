using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Animation _mainMenuAnimator;
    [SerializeField] AnimationClip _fadeInAnimation;
    [SerializeField] AnimationClip _fadeOutAnimation;
    [SerializeField] Text title;
    [SerializeField] Text tagline;


    public Events.EventFadeComplete OnMainMenuFadeComplete;
    public Events.EventFadeComplete OnLoadingScreenFadeComplete;
    private void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
        GameText.Instance.OnLanguageChanged.AddListener(HandleLanguageChanged);
        if (GameText.Instance.Initialized)
        {
            title.text = GameText.Instance.GetText("app_name");
            tagline.text = GameText.Instance.GetText("pressToBegin");
        }
    }


    void OnFadeOutComplete()
    {
        OnMainMenuFadeComplete.Invoke(true);
        gameObject.SetActive(false);
    }
    void OnFadeInComplete()
    {
        OnMainMenuFadeComplete.Invoke(false);
        UIManager.Instance.SetDummyCameraActive(true);
    }
    private void HandleLanguageChanged()
    {
        title.text = GameText.Instance.GetText("app_name");
        tagline.text = GameText.Instance.GetText("pressToBegin");
    }

    void HandleGameStateChanged (GameManager.GameState currentState, GameManager.GameState previousState)
    {
        if (previousState == GameManager.GameState.PREGAME && currentState == GameManager.GameState.PREGAME_RUNNING_SWITCH)
        {
            FadeOut();
        }
        if (previousState != GameManager.GameState.PREGAME && currentState == GameManager.GameState.PREGAME)
        {
            FadeIn();
        }
    }

    public void FadeIn()
    {
        gameObject.SetActive(true);
        _mainMenuAnimator.Stop();
        _mainMenuAnimator.clip = _fadeInAnimation;
        _mainMenuAnimator.Play();
    }
    public void FadeOut()
    {
        UIManager.Instance.SetDummyCameraActive(false);

        _mainMenuAnimator.Stop();
        _mainMenuAnimator.clip = _fadeOutAnimation;
        _mainMenuAnimator.Play();
    }
}
