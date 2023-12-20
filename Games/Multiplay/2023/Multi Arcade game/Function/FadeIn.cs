using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{    
    [Header("페이드 인 속도")]
    public float fadeInSpeed = 0.3f;

    Image background;

    void Awake()
    {
        background = GetComponent<Image>();
    }

    void OnEnable()
    {
        StartCoroutine(OnFadeIn());
    }

    IEnumerator OnFadeIn()
    {
        Color temp = background.color;

        temp.a = 0;
        background.color = temp;

        yield return null;

        while (background.color.a < 1)
        {
            temp.a += Time.deltaTime * fadeInSpeed;
            background.color = temp;
            yield return null;
        }

        temp.a = 1;
        background.color = temp;        
    }
}
