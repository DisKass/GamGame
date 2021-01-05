using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Dissolve : MonoBehaviour
{
    [System.Serializable]
    struct DissolveColors
    {
        [ColorUsage(true, true)] public Color common;
        [ColorUsage(true, true)] public Color fire;
        [ColorUsage(true, true)] public Color freeze;
    }
    [SerializeField] DissolveColors dissolveColors;
    [SerializeField] SpriteRenderer spriteRenderer;

    float fade = 1;
    float speed = 0.02f;
    public void Initialize(GameObject target)
    {
        transform.position = target.transform.position;

        SpriteRenderer targetSpriteRenderer = target.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = targetSpriteRenderer.sprite;
        spriteRenderer.sortingLayerID = targetSpriteRenderer.sortingLayerID;
        spriteRenderer.sortingOrder = targetSpriteRenderer.sortingOrder;


        Buff_Debuff_System targetBuffs = target.GetComponent<Buff_Debuff_System>();

        Color dissolveColor = dissolveColors.common;
        if (targetBuffs.currentBuffs.Burning) dissolveColor = dissolveColors.fire;
        if (targetBuffs.currentBuffs.Freezing) dissolveColor = Color.Lerp(dissolveColor, dissolveColors.freeze, 0.5f);
        SetColor(dissolveColor);
        StartCoroutine(Disolve());
    }
    public void SetColor(Color color)
    {
        spriteRenderer.material.SetColor("_Color", color);
    }
    public void SetScale(float scale)
    {
        spriteRenderer.material.SetFloat("_Scale", scale);
    }
    public void SetFade(float fade)
    {
        spriteRenderer.material.SetFloat("_Fade", fade);
    }
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
    IEnumerator Disolve()
    {
        FollowingCamera mainCamera = Camera.main.GetComponent<FollowingCamera>();
        mainCamera.setPostProcessing(true);
        while (fade > 0)
        {
            fade -= speed;
            spriteRenderer.material.SetFloat("_Fade", fade);
            yield return new WaitForFixedUpdate();
        }
        mainCamera.setPostProcessing(false);
        Destroy(gameObject);
    }
    //void Update()
    //{
    //    fade -= speed;
    //    spriteRenderer.material.SetFloat("_Fade", fade);
    //    if (fade <= 0) Destroy(gameObject);
    //}
}
