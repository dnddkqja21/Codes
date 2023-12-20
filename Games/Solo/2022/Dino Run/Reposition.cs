using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Reposition : MonoBehaviour
{
    public UnityEvent OnRandomSprite;

    // �Ѱ� ����
    float limitX = -10f;
    // �̵��� ����
    float moveToX = 24f;

    void LateUpdate()
    {
        if(transform.position.x > limitX) { return; }

        // ��� ��ǥ�� ������
        transform.Translate(moveToX, 0, 0, Space.Self);
        OnRandomSprite.Invoke();
    }
}
