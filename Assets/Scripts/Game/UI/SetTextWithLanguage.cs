using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class SetTextWithLanguage : MonoBehaviour
{
    void Start()
    {
        Text textComponent = gameObject.GetComponent<Text>();
        textComponent.text = GameText.Instance.GetText(textComponent.text);
        enabled = false;
    }
}
