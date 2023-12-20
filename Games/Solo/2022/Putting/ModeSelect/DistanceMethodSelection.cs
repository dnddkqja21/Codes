using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceMethodSelection : MonoBehaviour
{
    // �Ÿ� ���� ���
    [Header("�Ÿ� �Ʒ� ���")]
    public DistanceMethod distanceMethod;

    public void OnDistanceMethodSelection()
    {
        GameOption.Instance.distanceMethod = (int)distanceMethod;
    }
}
