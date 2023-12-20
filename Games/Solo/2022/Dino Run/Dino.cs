using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// �ʿ� ���
// 1. ���� (���� �Ŀ�)
// 2. ���� (���� �浹 �̺�Ʈ)
// 3. ��ֹ� (Ʈ���� �浹 �̺�Ʈ)
// 4. �ִϸ��̼�
// 5. ����

public enum State
{
    Idle,
    Run,
    Jump,
    Die
}

public class Dino : MonoBehaviour
{   
    // �ν����� ����
    [Header("���� ���� �Ŀ�")]
    public float startJumpPower;
    [Header("���� �Ŀ�")]
    public float jumpPower;
    [Header("�׶��� üũ")]
    public bool isGround;    
    [Header("���� �Ŀ� ���� ����")]
    public float interval = 0.1f;    
    [Header("����Ű")]
    public KeyCode space = KeyCode.Space;

    [Space(50)]
    // ����Ƽ �̺�Ʈ
    public UnityEvent OnHit;

    bool isLongJump;

    // ���� ������Ʈ
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
        // ���� ����� �Ƚ��忡��
        LongJump();
    }

    void ShortJump()
    {
        if ((Input.GetKeyDown(space) || Input.GetMouseButtonDown(0)) && isGround)
        {            
            // impulse : ������ ���� 1�����ӿ� ������� ���� ����.
            // �� ������ ������ ���� ����̶� ������Ʈ���� ó���ص� ������ ����.
            rigid.AddForce(Vector2.up * startJumpPower, ForceMode2D.Impulse);           
        }
    }

    void LongJump()
    {   
        // �� ������ ���� �پ����� ���� ����(���� ��) ������ ��ŭ �����̱� ������ !isGround�̴�.
        if (isLongJump && !isGround)
        {            
            jumpPower = Mathf.Lerp(jumpPower, 0, interval);
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);            
        }
    }

    void InputLongJumpKey()
    {        
        // ��ǲ�� ������Ʈ����        
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
        // ������ �ùķ��̼� �����ʰڴ�
        rigid.simulated = false;
        ChangeAnimation(State.Die);
        sound.PlaySound(SFX.Hit);
        OnHit.Invoke();
        Debug.Log("���ӿ���");
    }

    void ChangeAnimation(State state) 
    {        
        ani.SetInteger("State", (int)state);        
    }   
}
