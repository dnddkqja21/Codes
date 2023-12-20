using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    float tileSize;
    //float repositionGap = 3f;

    Collider2D myCollider;
    BoxCollider2D area;

    void Awake()
    {
        myCollider = GetComponent<Collider2D>();        
    }

    void Start()
    {
        // awake호출 시 게임 매니저 등등의 호출과 겹쳐 못가져오는 일 생기므로 호출 순서를 염두하여 스타트에서 호출한다.
        // 플레이어 자식 오브젝트 중 area가져와 박스 콜라이더 사이즈 대입 
        area = GameManager.Instance.player.GetComponentInChildren<BoxCollider2D>();
        tileSize = area.size.x;
        //Debug.Log(tileSize);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // 태그를 조건
        // Area 태그가 아닐 경우 리턴 즉, Area 태그에서 벗어났을 때만 아래 코드를 실행
        if(!collision.CompareTag("Area"))
        {
            return;
        }

        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 tilePos = transform.position;

        // 예전 코드
        #region 인풋 벡터에 의하여 맵을 이동시킬 시 나의 인풋이 0이라면 삼항 연사자에 의해 1이 나오기 때문에 나의 의도와 다른 맵이동 보임
         
        //// x,y 방향을 정규화
        //Vector3 playerDir = GameManager.Instance.player.inputVector;
        
        #endregion   
        
        // 나의 태그로 분기
        // Area 지역을 벗어난 게 "Ground" 또는 "Enemy"냐에 따라 다른 로직으로 이동
        switch (transform.tag)
        {
            case "Ground":
                // 인풋 벡터에 의한 맵이동이 아닌 단순히 플레이어의 위치를 이용한다.
                //// 축 간 거리 (플레이어 위치 - 타일 위치)
                float disX = playerPos.x - tilePos.x;
                float disY = playerPos.y - tilePos.y;
                //// x값이 0보다 작으면 -1 아니면 1
                float dirX = disX < 0 ? -1 : 1;
                float dirY = disY < 0 ? -1 : 1;
                // 이후에 절대값 계산
                disX = Mathf.Abs(disX);
                disY = Mathf.Abs(disY);

                // 플레이어와 타일맵의 거리 차이에서 x축이 y축보다 크면 수평이동                
                if (disX > disY)
                {
                    // 이동할 양으로 이동 (x축에 x축 방향 값을 준 후 거리를 대입함.
                    // 타일 사이즈를 2칸 옆으로 밀어야 하기 때문에 2를 곱한다.
                    transform.Translate(Vector3.right * dirX * tileSize * 2);
                }
                else if(disX < disY)
                {
                    transform.Translate(Vector3.up * dirY * tileSize * 2);
                }
                break;

            case "Enemy":
                // 콜라이더가 활성화일 때만 (몬스터일 경우 살아있는 동안)
                if(myCollider.enabled)
                {
                    // 플레이어와 타일의 방향으로 2배만큼 이동, 랜덤성 부여
                    Vector3 dir = playerPos - tilePos;
                    Vector3 random = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    transform.Translate(random + dir * 2);
                }
                break;
        }
    }
}
