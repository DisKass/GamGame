using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] Animation _loadingScreenAnimator;
    [SerializeField] AnimationClip _fadeInAnimation;
    [SerializeField] AnimationClip _fadeOutAnimation;


    public Events.EventFadeComplete OnLoadingScreenFadeComplete;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
    }

    private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        if (previousState == GameManager.GameState.CHANGELEVEL && currentState == GameManager.GameState.RUNNING)
        {
            FadeOut();
        }
    }

    void OnFadeOutComplete()
    {
        OnLoadingScreenFadeComplete.Invoke(true);
        gameObject.SetActive(false);
    }
    void OnFadeInComplete()
    {
        OnLoadingScreenFadeComplete.Invoke(false);
    }
    public void FadeIn()
    {
        gameObject.SetActive(true);
        _loadingScreenAnimator.Stop();
        _loadingScreenAnimator.clip = _fadeInAnimation;
        _loadingScreenAnimator.Play();
    }
    public void FadeOut()
    {

        _loadingScreenAnimator.Stop();
        _loadingScreenAnimator.clip = _fadeOutAnimation;
        _loadingScreenAnimator.Play();
    }
}
