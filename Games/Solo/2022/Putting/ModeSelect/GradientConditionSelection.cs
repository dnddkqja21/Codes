using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GradientConditionSelection : MonoBehaviour
{
    // 기울기 훈련 환경
    [Header("기울기 훈련 환경")]
    public GradientCondition gradientCondition;
    [Header("기울기 환경 텍스트")]
    public TextMeshProUGUI levelTitle;

    private void Start()
    {
        levelTitle.text = EnumToData.Instance.GradientTitleToKor((int)gradientCondition);
    }

    public void OnGradientConditionSelection()
    {
        GameOption.Instance.gradientCondition = (int)gradientCondition;
        //Debug.Log(GameOption.Instance.gradientCondition);
    }
}
