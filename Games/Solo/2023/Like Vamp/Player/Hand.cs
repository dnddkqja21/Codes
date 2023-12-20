using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool isLeft;
    public SpriteRenderer spriteRenderer;

    SpriteRenderer player;

    Vector3 rightPos = new Vector3(0.35f, -0.15f, 0);
    Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0);
    Quaternion leftPos = Quaternion.Euler(0, 0, -35);
    Quaternion leftPosReverse = Quaternion.Euler(0, 0, -135);

    void Awake()
    {
        // �θ��� ������Ʈ�� �����͵� �ڱ� �ڽ��� ��������Ʈ ������ ���� �������� ������
        // �� ��° ���� �����;� �Ѵ�.
        player = GetComponentsInParent<SpriteRenderer>()[1];    
    }

    void LateUpdate()
    {
        // �÷��̾��� �ø�x��
        bool isReverse = player.flipX;

        // ����
        if(isLeft)
        {
            transform.localRotation = isReverse ? leftPosReverse : leftPos;
            spriteRenderer.flipY = isReverse;
            spriteRenderer.sortingOrder = isReverse ? 4 : 6;
        }
        // ���Ÿ�
        else
        {
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
            spriteRenderer.flipX = isReverse;
            spriteRenderer.sortingOrder = isReverse ? 6 : 4;
        }
    }
}
