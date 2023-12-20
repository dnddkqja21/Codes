using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActualStartPointSelection : MonoBehaviour
{
    // ���� ���� ����
    [Header("���� ���� ����")]
    public ActualStartPoint actualStartPoint;
    [Header("���� ���� �ؽ�Ʈ")]
    public TextMeshProUGUI start;

    private void Start()
    {
        start.text = EnumToData.Instance.ActualStartToKor((int)actualStartPoint);
    }

    public void OnActualStartPointSelection()
    {
        GameOption.Instance.actualStartPoint = (int)actualStartPoint;
    }
}
