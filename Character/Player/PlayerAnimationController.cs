using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] Animator _playerAnimator;
    public void Move(bool value)
    {
        if (_playerAnimator != null)

        _playerAnimator.SetBool("Move", value);
    }
    public void Pause()
    {
        _playerAnimator.enabled = false;
    }
    public void Continue()
    {
        _playerAnimator.enabled = true;
    }
}
