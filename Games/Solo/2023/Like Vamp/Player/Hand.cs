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
        // 부모의 컴포넌트를 가져와도 자기 자신의 스프라이트 렌더러 또한 가져오기 때문에
        // 두 번째 것을 가져와야 한다.
        player = GetComponentsInParent<SpriteRenderer>()[1];    
    }

    void LateUpdate()
    {
        // 플레이어의 플립x축
        bool isReverse = player.flipX;

        // 근접
        if(isLeft)
        {
            transform.localRotation = isReverse ? leftPosReverse : leftPos;
            spriteRenderer.flipY = isReverse;
            spriteRenderer.sortingOrder = isReverse ? 4 : 6;
        }
        // 원거리
        else
        {
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
            spriteRenderer.flipX = isReverse;
            spriteRenderer.sortingOrder = isReverse ? 6 : 4;
        }
    }
}
