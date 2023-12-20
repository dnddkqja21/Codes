using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestResult : MonoBehaviour
{
    [Header("타이틀 텍스트")]
    public TextMeshProUGUI title;
    [Header("훈련 수")]
    public TextMeshProUGUI tranningCount;
    [Header("성공 수")]
    public TextMeshProUGUI successCount;
    [Header("성공률")]
    public TextMeshProUGUI successRate;

    private void Awake()
    {
        SaveResult();
    }
    void Start()
    {
        if(GameOption.Instance.selectedMode == (int)GameMode.FREE)
        {
            title.text = "자유 훈련 결과";
        }
        else
        {
            title.text = EnumToData.Instance.TranningModeToKor(GameOption.Instance.tranningMode) + " 결과";
        }
        tranningCount.text = GameOption.Instance.TranningCount.ToString() + "개";
        successCount.text = GameOption.Instance.successCount.ToString() + "개";
        successRate.text = string.Format("{0:F1}" , (100 / ((float)GameOption.Instance.TranningCount / (float)GameOption.Instance.successCount))) + "%";
    }

    public void SaveResult()
    {
        // 추후 기록 저장은 여기서 구현
    }
}
