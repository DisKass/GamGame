using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideByTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player" || collision.tag == "Enemy")
        {       
            StopAllCoroutines();
            StartCoroutine("ChangeColor", 0.4f);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Enemy")
        {
            StopAllCoroutines();
            StartCoroutine("ChangeColor", 1f);
        }
    }

    IEnumerator ChangeColor(float value)
    {
        Color color = GetComponent<SpriteRenderer>().color;
        float step = (value - color.a) / 10;
        float currentA = color.a;
        for (int i = 0; i < 10; i++)
        {
            currentA += step;
            GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, currentA);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
