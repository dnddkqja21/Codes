using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestResult : MonoBehaviour
{
    [Header("Ÿ��Ʋ �ؽ�Ʈ")]
    public TextMeshProUGUI title;
    [Header("�Ʒ� ��")]
    public TextMeshProUGUI tranningCount;
    [Header("���� ��")]
    public TextMeshProUGUI successCount;
    [Header("������")]
    public TextMeshProUGUI successRate;

    private void Awake()
    {
        SaveResult();
    }
    void Start()
    {
        if(GameOption.Instance.selectedMode == (int)GameMode.FREE)
        {
            title.text = "���� �Ʒ� ���";
        }
        else
        {
            title.text = EnumToData.Instance.TranningModeToKor(GameOption.Instance.tranningMode) + " ���";
        }
        tranningCount.text = GameOption.Instance.TranningCount.ToString() + "��";
        successCount.text = GameOption.Instance.successCount.ToString() + "��";
        successRate.text = string.Format("{0:F1}" , (100 / ((float)GameOption.Instance.TranningCount / (float)GameOption.Instance.successCount))) + "%";
    }

    public void SaveResult()
    {
        // ���� ��� ������ ���⼭ ����
    }
}
