using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Reposition : MonoBehaviour
{
    public UnityEvent OnRandomSprite;

    // 한계 지점
    float limitX = -10f;
    // 이동할 지점
    float moveToX = 24f;

    void LateUpdate()
    {
        if(transform.position.x > limitX) { return; }

        // 상대 좌표를 움직임
        transform.Translate(moveToX, 0, 0, Space.Self);
        OnRandomSprite.Invoke();
    }
}
