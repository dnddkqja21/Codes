using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InProgressGradientTextSet : MonoBehaviour
{
    [Header("훈련 중 그린 기울기 텍스트")]
    public TextMeshProUGUI[] inProgressGradientTexts;

    public void OnGradientTextSet()
    {
        for (int i = 0; i < inProgressGradientTexts.Length; i++)
        {
            inProgressGradientTexts[i].text = GameOption.Instance.gradients[i].ToString();
        }
    }
}
