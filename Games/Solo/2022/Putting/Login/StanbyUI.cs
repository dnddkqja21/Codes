using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StanbyUI : MonoBehaviour
{
    [Header("배경")]
    public Image background;
    [Header("애니메이션")]
    public Animation ani;
    float fadeOutSpeed = 0.5f;
    bool isClicked = false;

    public void HidePopup()
    {
        if(isClicked)
        {
            return;
        }
        StartCoroutine(FadeOut());        
    }

    IEnumerator FadeOut()
    {
        isClicked = true;

        yield return new WaitForSeconds(0.3f);

        ani.clip = ani.GetClip("OffTitle");
        ani.Play();

        Color temp = background.color;
        while (background.color.a > 0)
        {
            temp.a -= Time.deltaTime * fadeOutSpeed;
            background.color = temp;
            yield return null;
        }

        temp.a = 0;
        background.color = temp;

        gameObject.SetActive(false);
    }

    public void Initialize()
    {
        gameObject.SetActive(true);

        isClicked = false;

        Color temp = background.color;
        temp.a = 1;
        background.color = temp;

        ani.clip = ani.GetClip("OnTitle");
        ani.Play();
    }
}
