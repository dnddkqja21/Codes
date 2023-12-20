using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActualGradientSelection : MonoBehaviour
{
    // 실전 기울기 선택
    [Header("실전 기울기")]
    public ActualGradient actualGradient;
    [Header("기울기 텍스트")]
    public TextMeshProUGUI gradient;

    private void Start()
    {
        gradient.text = EnumToData.Instance.ActualGradientToKor((int)actualGradient);
    }

    public void OnActualGradientSelection()
    {
        GameOption.Instance.actualGradient = (int)actualGradient;
    }
}
