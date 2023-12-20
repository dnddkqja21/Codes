using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [Header("���� ����")]
    public float scanRange;
    [Header("���̾�")]
    public LayerMask targetLayer;
    [Header("�浹 ������")]
    public RaycastHit2D[] hits;
    [Header("���� ����� Ÿ��")]
    public Transform target;

    void FixedUpdate()
    {
        // ĳ���� ��ġ, ����, ����, �Ÿ�, ���̾�
        hits = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0 , targetLayer);

        target = GetTarget();
    }

    Transform GetTarget()
    {
        // ���� ����� ��
        Transform result = null;
        // ������ �Ÿ�
        float dis = 100;

        foreach(var item in hits)
        {
            // �÷��̾��� ��ġ�� ������ �ϳ��� ��ġ�� �Ÿ� ����
            Vector3 myPos = transform.position;
            Vector3 targetPos = item.transform.position;
            float curDis = Vector3.Distance(myPos, targetPos);
            
            // �ݺ��� ���� ���� �Ÿ��� ������ �Ÿ����� �۴ٸ� ������ �Ÿ��� �� ���� ����
            // ���������� ���� ����� �Ÿ��� item�� ����� target�� �ȴ�.
            if(curDis < dis) 
            {
                dis = curDis;
                result = item.transform;
            }
        }
        return result;
    }
}
