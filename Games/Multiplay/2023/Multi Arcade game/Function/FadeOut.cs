using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 페이드 아웃
/// </summary>

public class FadeOut : MonoBehaviour
{
    [Header("대기 시간")]
    public float waitTime = 0.5f;
    [Header("페이드 아웃 속도")]
    public float fadeOutSpeed = 0.3f;

    Image background;

    void Awake()
    {
        background = GetComponent<Image>();
    }

    void OnEnable()
    {
        StartCoroutine(OnFadeOut());
    }

    IEnumerator OnFadeOut()
    {
        Color temp = background.color;

        temp.a = 1;
        background.color = temp;

        yield return new WaitForSeconds(waitTime);


        while (background.color.a > 0)
        {
            temp.a -= Time.deltaTime * fadeOutSpeed;
            background.color = temp;
            yield return null;
        }

        temp.a = 0;
        background.color = temp;

        background.gameObject.SetActive(false);
    }
}
