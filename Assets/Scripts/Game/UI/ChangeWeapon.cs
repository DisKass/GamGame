using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWeapon : MonoBehaviour
{
    CharacterStats characterStats;
    WeaponManager weaponManager;

    private void Start()
    {
        characterStats = Player.Instance.GetComponent<CharacterStats>();
        weaponManager = WeaponManager.Instance;
    }
    public void Change()
    {
        Player.Instance.ChangeWeapon();
    }
}
