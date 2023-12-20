using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    // 컴포넌트 연결
    [Header("리지드 바디")]
    public Rigidbody2D rigid;
    [Header("스프라이트 렌더러")]
    public SpriteRenderer sprite;
    [Header("애니메이터")]
    public Animator ani;

    public Scanner scanner;
    public Hand[] hands;
    public RuntimeAnimatorController[] aniController;

    [Space(30)]    
    [Header("인풋 벡터 모니터")]
    public Vector2 inputVector;

    [Header("이동 속도")]
    public float moveSpeed;

    #region 과거의 인풋 시스템
    /*
    void Update()
    {
        // (Raw : 1 또는 0으로 딱 떨어짐) 게임의 기획에 따라 움직임 제어
        inputVector.x = Input.GetAxisRaw("Horizontal");
        inputVector.y = Input.GetAxisRaw("Vertical");
    }
    */
    #endregion

    void Awake()
    {
        scanner = GetComponent<Scanner>();
        // 비활성화 되어있는 오브젝트도 가져올 수 있음
        hands = GetComponentsInChildren<Hand>(true);
    }

    void OnEnable()
    {
        moveSpeed *= Character.Speed;
        ani.runtimeAnimatorController = aniController[GameManager.Instance.playerID];    
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;

        MovePlayer();
    }

    // 다음 프레임으로 넘어가기 직전에 호출
    void LateUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;

        MoveAnimation();
        FlipSprite();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.Instance.isLive)
            return;

        GameManager.Instance.health -= Time.deltaTime * 10;

        if(GameManager.Instance.health < 0)
        {
            // 자식 오브젝트 3번 째 것부터 비활성화
            for (int i = 2; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            ani.SetTrigger("Dead");
            GameManager.Instance.GameOver();
        }
    }

    private void FlipSprite()
    {
        if (inputVector.x != 0)
        {
            // 부등호가 참이면 true
            sprite.flipX = inputVector.x < 0;
        }
    }

    private void MoveAnimation()
    {
        // 방향 관계 없이 크기만 가져옴
        ani.SetFloat("Speed", inputVector.magnitude);
    }

    void MovePlayer()
    {
        #region 물리 이동 종류
        /*
        // 1. 힘 
        rigid.AddForce(inputVector);

        // 2. 속력 제어
        rigid.velocity = inputVector;
        */

        #endregion
        // 3. 위치 기반 이동 (현재 위치에서 움직일 위치)

        // 방향만을 가지도록 정규화
        // 대각선 이동의 경우 피타고라스 법칙에 의해 1보다 큰 값이 되므로 대각선 이동이 더 빨라지므로 정규화가 필요하다.
        //Vector2 nextVector = inputVector.normalized * moveSpeed * Time.fixedDeltaTime;

        // 새로운 인풋 시스템 패키지를 사용할 경우 인스펙터에서 정규화시키기 때문에 스크립트 내에서 할 필요가 없다.
        Vector2 nextVector = inputVector * moveSpeed * Time.fixedDeltaTime;

        rigid.MovePosition(rigid.position + nextVector);
    }

    // 인풋 시스템 패키지 사용
    void OnMove(InputValue value)
    {
        inputVector = value.Get<Vector2>();
    }
}
