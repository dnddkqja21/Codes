using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveObject : MonoBehaviour
{
    [SerializeField] 
    float heightFrom = 0.25f;
    [SerializeField] 
    float heightTo = 1.0f;

    Material material;

    void Awake()
    {
        material = GetComponent<Renderer>().material;
    }    

    void OnEnable()
    {
        StartCoroutine(Solidify());
    }

    public void DoDissolve()
    {
        StartCoroutine(Dissolve());
    }

    IEnumerator Dissolve()
    {
        float elapsedTime = 0f;
        float duration = 2f;

        while (elapsedTime < duration)
        {
            float height = Mathf.Lerp(heightTo, heightFrom, elapsedTime / duration);
            SetHeight(height);

            elapsedTime += Time.deltaTime;
            yield return null;
        }        
        SetHeight(heightFrom);
        gameObject.SetActive(false);
    }

    IEnumerator Solidify()
    {
        float elapsedTime = 0f;
        float duration = 2f;

        while (elapsedTime < duration)
        {
            float height = Mathf.Lerp(heightFrom, heightTo, elapsedTime / duration);
            SetHeight(height);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        SetHeight(heightTo);
    }

    void SetHeight(float height)
    {
        material.SetFloat("_CutoffHeight", height);
    }
}
