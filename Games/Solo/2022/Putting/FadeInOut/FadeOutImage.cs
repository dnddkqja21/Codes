using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutImage : MonoBehaviour
{
    [Header("페이드 아웃 이미지")]
    public Image background;
    [Header("대기 시간")]
    public float waitTime = 0.5f;
    [Header("페이드 아웃 속도")]
    public float fadeOutSpeed = 0.65f;

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
