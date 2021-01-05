using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructToParticles : Singleton<DestructToParticles>
{
     ObjectsPool objectsPool;
    [SerializeField] GameObject effectObject;

    protected override void Awake()
    {
        base.Awake();
        objectsPool = new ObjectsPool(effectObject);
    }

    public void Destruct(Sprite sprite, Vector3 position)
    {
        ParticleSystem p = objectsPool.GetElement().GetComponent<ParticleSystem>();
        var sh = p.shape;
        sh.sprite = sprite;
        sh.texture = sprite.texture;
        p.transform.position = position;
        StartCoroutine(Disable(p.gameObject, 0.2f));
    }

    IEnumerator Disable(GameObject target, float time)
    {
        yield return new WaitForSeconds(time);
        target.SetActive(false);
    }
}
