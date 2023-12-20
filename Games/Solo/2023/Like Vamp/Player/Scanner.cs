using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [Header("감지 범위")]
    public float scanRange;
    [Header("레이어")]
    public LayerMask targetLayer;
    [Header("충돌 정보들")]
    public RaycastHit2D[] hits;
    [Header("가장 가까운 타겟")]
    public Transform target;

    void FixedUpdate()
    {
        // 캐스팅 위치, 범위, 방향, 거리, 레이어
        hits = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0 , targetLayer);

        target = GetTarget();
    }

    Transform GetTarget()
    {
        // 가장 가까운 적
        Transform result = null;
        // 임의의 거리
        float dis = 100;

        foreach(var item in hits)
        {
            // 플레이어의 위치와 아이템 하나의 위치로 거리 산정
            Vector3 myPos = transform.position;
            Vector3 targetPos = item.transform.position;
            float curDis = Vector3.Distance(myPos, targetPos);
            
            // 반복문 돌며 현재 거리가 임의의 거리보다 작다면 임의의 거리에 그 값을 대입
            // 최종적으로 가장 가까운 거리의 item이 가까운 target이 된다.
            if(curDis < dis) 
            {
                dis = curDis;
                result = item.transform;
            }
        }
        return result;
    }
}
