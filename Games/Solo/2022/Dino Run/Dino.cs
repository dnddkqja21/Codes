using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 필요 기능
// 1. 점프 (점프 파워)
// 2. 착지 (물리 충돌 이벤트)
// 3. 장애물 (트리거 충돌 이벤트)
// 4. 애니메이션
// 5. 사운드

public enum State
{
    Idle,
    Run,
    Jump,
    Die
}

public class Dino : MonoBehaviour
{   
    // 인스펙터 공개
    [Header("시작 점프 파워")]
    public float startJumpPower;
    [Header("점프 파워")]
    public float jumpPower;
    [Header("그라운드 체크")]
    public bool isGround;    
    [Header("점프 파워 감소 간격")]
    public float interval = 0.1f;    
    [Header("점프키")]
    public KeyCode space = KeyCode.Space;

    [Space(50)]
    // 유니티 이벤트
    public UnityEvent OnHit;

    bool isLongJump;

    // 보유 컴포넌트
    Rigidbody2D rigid;
    Animator ani;
    SoundManager sound;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        sound = GetComponent<SoundManager>();
    }

    void Start()
    {
        sound.PlaySound(SFX.Start);
    }

    void Update()
    {
        if(!GameManager.isLive) { return; }
        ShortJump();
        InputLongJumpKey();
    }
    void FixedUpdate()
    {
        if (!GameManager.isLive) { return; }
        // 물리 계산은 픽스드에서
        LongJump();
    }

    void ShortJump()
    {
        if ((Input.GetKeyDown(space) || Input.GetMouseButtonDown(0)) && isGround)
        {            
            // impulse : 누르는 순간 1프레임에 즉발적인 힘을 가함.
            // 한 프레임 정도는 물리 계산이라도 업데이트에서 처리해도 문제가 없다.
            rigid.AddForce(Vector2.up * startJumpPower, ForceMode2D.Impulse);           
        }
    }

    void LongJump()
    {   
        // 롱 점프는 땅에 붙어있지 않은 동안(점프 중) 누르는 만큼 점프이기 때문에 !isGround이다.
        if (isLongJump && !isGround)
        {            
            jumpPower = Mathf.Lerp(jumpPower, 0, interval);
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);            
        }
    }

    void InputLongJumpKey()
    {        
        // 인풋은 업데이트에서        
        isLongJump = Input.GetKey(space) || Input.GetMouseButton(0);        
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(!isGround)
        {
            ChangeAnimation(State.Run);
            jumpPower = 1;
            sound.PlaySound(SFX.Land);
        }
        isGround = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isGround = false;
        ChangeAnimation(State.Jump);
        sound.PlaySound(SFX.Jump);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 물리를 시뮬레이션 하지않겠다
        rigid.simulated = false;
        ChangeAnimation(State.Die);
        sound.PlaySound(SFX.Hit);
        OnHit.Invoke();
        Debug.Log("게임오버");
    }

    void ChangeAnimation(State state) 
    {        
        ani.SetInteger("State", (int)state);        
    }   
}
