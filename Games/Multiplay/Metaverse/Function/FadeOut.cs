using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    [Header("��� �ð�")]
    public float waitTime = 0.5f;
    [Header("���̵� �ƿ� �ӵ�")]
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
