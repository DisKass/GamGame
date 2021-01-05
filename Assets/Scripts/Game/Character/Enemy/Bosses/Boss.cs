using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    OnGUIHealthBar myHealthbar;
    [SerializeField] Assets.Scripts.Game.Items.ItemPlatform itemPlatform; 
    protected new void Awake()
    {
        base.Awake();
        myHealthbar = UIManager.Instance.BossHealthbar.GetComponent<OnGUIHealthBar>();
        myHealthbar.character = this;
        myHealthbar.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        if (myHealthbar != null)
        {
            myHealthbar.character = null;
            myHealthbar.gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        OnDestroy();
    }
    public override void Die()
    {
        if (CharacterStats.IsDead) return;
        base.Die();
        //Instantiate(itemPlatform, transform.position, Quaternion.identity);
    }
}
