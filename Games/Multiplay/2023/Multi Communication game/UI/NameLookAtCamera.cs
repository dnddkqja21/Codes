using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ǹ� �̸� UI�� �׻� ī�޶� ������ ��
/// </summary>

public class NameLookAtCamera : MonoBehaviour
{
    // ���� ���⼭ ���ö���¡, 
    // ����غ� ���� : �ǹ� �̸� �����ͼ� tmp�� ����
    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
