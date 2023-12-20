using System.Collections;
using UnityEngine;
using TMPro;

public class FadeOutText : MonoBehaviour
{
    [Header("페이드 아웃 텍스트")]
    public TextMeshProUGUI tmpro;
    [Header("대기 시간")]
    public float waitTime = 1f;
    [Header("페이드 아웃 속도")]
    public float fadeOutSpeed = 0.65f;

    private void OnEnable()
    {        
        StartCoroutine(OnFadeOut());        
    }

    IEnumerator OnFadeOut()
    {
        //tmpro.alpha = 1;
        Color temp = tmpro.color;

        temp.a = 1;
        tmpro.color = temp;

        yield return new WaitForSeconds(waitTime);

        
        //tmpro.alpha -= Time.deltaTime * fadeOutSpeed;
        while(tmpro.color.a > 0)
        {
            temp.a -= Time.deltaTime * fadeOutSpeed;            
            tmpro.color = temp;
            yield return null;
        }
        
        //tmpro.alpha = 0;
        temp.a = 0;
        tmpro.color = temp;

        tmpro.enabled = false;
        tmpro.gameObject.SetActive(false);
    }
}
