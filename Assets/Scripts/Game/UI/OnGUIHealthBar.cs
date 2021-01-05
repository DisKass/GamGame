using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class OnGUIHealthBar : MonoBehaviour
{
    Slider healthbar;
    [SerializeField] Text healthText;
    [SerializeField] bool IsPlayer = false;
    CharacterStats characterStats;
    public ICharacter character;

    private void Awake()
    {
        healthbar = GetComponent<Slider>();
    }
    void Initialize()
    {
        characterStats.OnCharacterPropertyChanged.AddListener(HandleCharacterPropertyChanged);
        healthbar.maxValue = characterStats.MaxHealth;
        healthbar.value = characterStats.Health;
        healthText.text = characterStats.Health + "/" + characterStats.MaxHealth;
    }
    private void OnEnable()
    {
        StartCoroutine(searchCharacter());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    IEnumerator searchCharacter()
    {
        if (IsPlayer)
        {
            yield return new WaitWhile(() => Player.Instance?.CharacterStats == null);
            characterStats = Player.Instance.CharacterStats;
        }
        else
        {
            yield return new WaitWhile(() => character == null);
            characterStats = character.CharacterStats;
        }
        Initialize();
    }

    private void HandleCharacterPropertyChanged(CharacterStats.PropertyID property, object value)
    {
        if (property == CharacterStats.PropertyID.MAXHEALTH)
        {
            healthbar.maxValue = characterStats.MaxHealth;
            UpdateHealth();
        }
        if (property == CharacterStats.PropertyID.HEALTH)
        {
            UpdateHealth();
        }
    }
    void UpdateHealth()
    {
        if (characterStats.Health > 0)
        {
            healthbar.value = characterStats.Health;
            healthText.text = characterStats.Health + "/" + characterStats.MaxHealth;
        }
        else
        {
            healthbar.value = 0;
            healthText.text = 0 + "/" + characterStats.MaxHealth;
        }
    }
}
