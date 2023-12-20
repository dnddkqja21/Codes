using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    float moveSpeed = 1f;

    float alphaSpeed = 1f;

    TextMeshPro dmgText;

    Color alpha;

    public int damage;
    
    void Start()
    {
        dmgText = GetComponent<TextMeshPro>();
        alpha = dmgText.color;
        Invoke("DestroyText", 3f);
        dmgText.text = damage.ToString();
    }

    
    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        alpha.a = Mathf.Lerp(alpha.a, 0, alphaSpeed * Time.deltaTime);
        dmgText.color = alpha;
    }
    
    void DestroyText()
    {
        Destroy(gameObject);
    }
}
