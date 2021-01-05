using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Pop_Ups : Singleton<Pop_Ups>
{
    List<TextMeshPro> textMeshContainer;
    Transform popUpsContainer;
    [SerializeField] TextMeshPro popUpPrefab;
    protected override void Awake()
    {
        base.Awake();
        textMeshContainer = new List<TextMeshPro>();
        popUpsContainer = new GameObject("PopUpsContainer").transform;
        popUpsContainer.SetParent(transform);
    }
    public void Heal(int value, Transform target)
    {
        Color color = Color.green;
        float fontSize = 0.7f;
        GetPopUp(value.ToString(), fontSize, target, color, 0.5f);
    }
    public void HandleOnDamageRecieved(Bullet.DamageInfo damageInfo, Transform target, GameObject source)
    {
        Color color = Color.white;
        float fontSize = 0.7f;
        if (damageInfo.damageType.IsDot)
        {
            fontSize -= 0.15f;
        }
        if (damageInfo.damageType.IsFire && damageInfo.damageType.IsDot)
        {
            color = Color.HSVToRGB(0.077f, 1, 0.89f);
        }
        if (damageInfo.damageType.IsIce)
        {
            color = Color.HSVToRGB(0.567f, 1, 0.89f);
        }
        if (damageInfo.damageType.IsCritical)
        {
            fontSize += 0.3f;
            color = Color.red;
        }
        GetPopUp(damageInfo.Damage.ToString(), fontSize, target, color, 0.5f);
    }
    public TextMeshPro GetPopUp(string text, float fontSize, Transform parent, Color color, float positionSpread = 0f)
    {
        foreach(TextMeshPro tm in textMeshContainer)
        {
            if (!tm.IsActive())
            {
                tm.text = text;
                tm.transform.position = parent.position +
                    (Vector3)Random.insideUnitCircle * positionSpread;
                tm.fontSize = fontSize;
                tm.color = color;
                tm.enabled = true;
                StartCoroutine(moveText(tm));
                return tm;
            }
        }
        TextMeshPro textMesh = Instantiate(popUpPrefab);
        textMeshContainer.Add(textMesh);
        textMesh.text = text;
        textMesh.transform.SetParent(popUpsContainer);
        //textMesh.transform.parent = parent;
        textMesh.transform.position = parent.position + 
            (Vector3)Random.insideUnitCircle*positionSpread;
        textMesh.color = color;
        textMesh.fontSize = fontSize;
        StartCoroutine(moveText(textMesh));
        return textMesh;
    }
    IEnumerator moveText(TextMeshPro textMesh)
    {
        float endTime = Time.time + 0.5f;
        Vector3 moving = 2f * Vector2.up;
        while (endTime > Time.time)
        {
            textMesh.transform.position += Time.fixedDeltaTime * moving;
            yield return new WaitForFixedUpdate();
        }
        textMesh.enabled = false;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        foreach (TextMeshPro tm in textMeshContainer)
        {
            tm.enabled = false;
        }
    }
}
