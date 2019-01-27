using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefugeeColor : MonoBehaviour
{
    public SpriteRenderer sprite;

    Color A;

    Color B;

    Coroutine runAlpha;

    private void Awake()
    {
        B = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
        A = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0f);
        sprite.color = A;
    }


    public void RunAlpha(float time2Run)
    {
        if (runAlpha != null)
        {
            StopCoroutine(runAlpha);
        }
        runAlpha = StartCoroutine(AlphaRutine(time2Run));
    }

    IEnumerator AlphaRutine(float time2Wait)
    {
        float timelol = 0;
        while (timelol < time2Wait)
        {
            sprite.color = Color.Lerp(A, B, timelol / time2Wait);
            timelol += Time.deltaTime;
            yield return null;
        }
        sprite.color = B;
    }
}
