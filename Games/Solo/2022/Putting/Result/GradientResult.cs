using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GradientResult : MonoBehaviour
{
    [Header("훈련 환경")]
    public TextMeshProUGUI condition;
    [Header("단계 별 훈련 수")]
    public TextMeshProUGUI[] tranningCountForL;
    [Header("단계 별 성공 수")]
    public TextMeshProUGUI[] successCountForL;
    [Header("단계 별 성공률")]
    public TextMeshProUGUI[] successRateForL;   
}
