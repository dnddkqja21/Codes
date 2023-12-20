using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceLevelSelection : MonoBehaviour
{
    [Header("�Ÿ� �Ʒ� �ܰ�")]
    public DistanceLevel distanceLevel;

    // �Ÿ� �Ʒ� ���̵�
    public void OnDistanceLevelSelection()
    {
        GameOption.Instance.distanceLevel = (int)distanceLevel;
    }
}
