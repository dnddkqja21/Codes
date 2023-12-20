using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeInImage : MonoBehaviour
{
    [Header("페이드 인 이미지")]
    public Image background;
    [Header("대기 시간")]
    public float waitTime = 0.5f;
    [Header("페이드 인 속도")]
    public float fadeOutSpeed = 0.65f;

    string nextScene = "1. Lobby";

    private void OnEnable()
    {
        StartCoroutine(OnFadeIn());        
    }

    IEnumerator OnFadeIn()
    {
        Color temp = background.color;

        temp.a = 0;
        background.color = temp;

        yield return new WaitForSeconds(waitTime);        

        while (background.color.a < 1)
        {
            temp.a += Time.deltaTime * fadeOutSpeed;
            background.color = temp;
            yield return null;
        }
        temp.a = 1;
        background.color = temp;
        SceneManager.LoadScene(nextScene);
    }
}
