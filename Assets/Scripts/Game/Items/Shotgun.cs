using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    public class Shotgun : MonoBehaviour, IItemEffect
    {
        public void Initialize(InitializeReason reason)
        {
            transform.SetParent(CharacterInventory.Instance.transform);
            //Player.Instance.CharacterStats.MaxHealth += 500;
            if (reason == InitializeReason.PICKUPPED)
            {
                Player.Instance.CharacterStats.ReloadMultiplier += 0.5f;
                Player.Instance.CharacterStats.BulletSpeed += 20f;
                Player.Instance.CharacterStats.BulletLifeTime -= 0.7f;
            }
            WeaponManager.Instance.spread += 45;
            
            WeaponManager.Instance._currentWeapon.AddGunpoint();
            WeaponManager.Instance._currentWeapon.AddGunpoint();
            WeaponManager.Instance._currentWeapon.AddGunpoint();
            WeaponManager.Instance._currentWeapon.AddGunpoint();
            gameObject.SetActive(false);
        }
    }
}
