using System.Collections;
using UnityEngine;

/// <summary>
/// πÃ¥œ∏  »Æ¿Â
/// </summary>

public class Expand : MonoBehaviour
{
    public RectTransform rectTransform;
    
    public float targetWidth;
    public float targetHeight;
    float initialWidth;
    float initialHeight;
    public float duration;

    private Coroutine expandCoroutine;
    private bool isExpanding = false;

    void Start()
    {
        initialWidth = rectTransform.rect.width;
        initialHeight = rectTransform.rect.height;
    }

    public void ToggleExpand()
    {
        if (expandCoroutine != null)
        {
            StopCoroutine(expandCoroutine);
        }

        if (isExpanding)
        {
            expandCoroutine = StartCoroutine(ContractCoroutine());            
        }
        else
        {
            expandCoroutine = StartCoroutine(ExpandCoroutine());
        }

        isExpanding = !isExpanding;        
    }

    IEnumerator ExpandCoroutine()
    {
        initialWidth = rectTransform.rect.width;
        initialHeight = rectTransform.rect.height;
        float currentTime = 0f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float normalizedTime = currentTime / duration;
            float newWidth = Mathf.Lerp(initialWidth, targetWidth, normalizedTime);
            float newHeight = Mathf.Lerp(initialHeight, targetHeight, normalizedTime);
            rectTransform.sizeDelta = new Vector2(newWidth, newHeight);
            yield return null;
        }        
    }

    IEnumerator ContractCoroutine()
    {        
        float currentTime = 0f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float normalizedTime = currentTime / duration;
            float newWidth = Mathf.Lerp(targetWidth, initialWidth, normalizedTime);
            float newHeight = Mathf.Lerp(targetHeight, initialHeight, normalizedTime);
            rectTransform.sizeDelta = new Vector2(newWidth, newHeight);
            yield return null;
        }        
    }
}