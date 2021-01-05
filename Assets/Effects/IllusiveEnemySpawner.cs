using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class IllusiveEnemySpawner : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    public Enemy Enemy { set
        {
            enemy = value;
            GetComponent<SpriteRenderer>().sprite = enemy.GetComponent<SpriteRenderer>().sprite;
        } }
    private void Start()
    {
        LevelStateController.OnLevelStateChanged.AddListener((a) =>
        {
            if (a == LevelStateController.LevelState.ACTION)
                StartCoroutine(FadeIn());
        });
        if (LevelStateController.Instance.CurrentLevelState == LevelStateController.LevelState.ENDING)
            Destroy(gameObject);

    }

    IEnumerator FadeIn()
    {
        Material material = GetComponent<SpriteRenderer>().material;
        for (float fade = 0; fade < 1; fade += 0.05f)
        {
            material.SetFloat("_fade", fade);
            yield return new WaitForEndOfFrame();
        }
        enemy.gameObject.SetActive(true);
        Destroy(gameObject);
    }
}
