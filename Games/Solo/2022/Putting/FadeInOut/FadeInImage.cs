using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeInImage : MonoBehaviour
{
    [Header("���̵� �� �̹���")]
    public Image background;
    [Header("��� �ð�")]
    public float waitTime = 0.5f;
    [Header("���̵� �� �ӵ�")]
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
