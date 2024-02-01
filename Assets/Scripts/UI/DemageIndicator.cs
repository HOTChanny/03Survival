using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour //껐다켰다 반복
{
    public Image image;
    public float flashSpeed;

    private Coroutine coroutine; //코루틴 공부 다시...

    public void Flash()
    {
        if (coroutine != null) // 코루틴이 null이 아니면
        {
            StopCoroutine(coroutine); // 코루틴 정지
        }

        image.enabled = true; // 이미 동작한 적 있으면 true
        image.color = Color.red;
        coroutine = StartCoroutine(FadeAway());
    }

    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;
        float a = startAlpha;

        while (a > 0.0f)
        {
            a -= (startAlpha / flashSpeed) * Time.deltaTime; // 한번에 깎아내릴값
            image.color = new Color(1.0f, 0.0f, 0.0f, a);
            yield return null;
        }

        image.enabled = false; // 데미지 입어서 화면 빨개졌다 다시 원상태로 (켰다끔)
    }
}