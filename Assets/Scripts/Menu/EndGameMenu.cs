using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameMenu : MonoBehaviour
{
    [SerializeField] Button RestartButton;
    [SerializeField] Button QuitButton;
    [SerializeField] Text victory;
    [SerializeField] Text defeat;
    public bool IsVictory
    {
        set
        {
            victory.gameObject.SetActive(value);
            defeat.gameObject.SetActive(!value);
        }
    }
    void OnEnable()
    {
        IsVictory = !Player.Instance.CharacterStats.IsDead;
    }
    public void Initialize()
    {
        RestartButton.onClick.AddListener(HandleRestartClick);
        QuitButton.onClick.AddListener(HandleQuitClick);
    }
    void HandleRestartClick()
    {
        GameManager.Instance.RestartGame();
    }
    void HandleQuitClick()
    {
        GameManager.Instance.QuitGame();
    }
}
