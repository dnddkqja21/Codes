using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DistanceResult : MonoBehaviour
{
    [Header("단계")]
    public TextMeshProUGUI level;
    [Header("방식")]
    public TextMeshProUGUI method;
    [Header("미터 별 훈련 수")]
    public TextMeshProUGUI[] tranningCountForM;
    [Header("미터 별 성공 수")]
    public TextMeshProUGUI[] successCountForM;
    [Header("미터 별 성공률")]
    public TextMeshProUGUI[] successRateForM;    
}
