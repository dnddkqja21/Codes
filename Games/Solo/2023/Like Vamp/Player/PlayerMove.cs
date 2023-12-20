using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    // ������Ʈ ����
    [Header("������ �ٵ�")]
    public Rigidbody2D rigid;
    [Header("��������Ʈ ������")]
    public SpriteRenderer sprite;
    [Header("�ִϸ�����")]
    public Animator ani;

    public Scanner scanner;
    public Hand[] hands;
    public RuntimeAnimatorController[] aniController;

    [Space(30)]    
    [Header("��ǲ ���� �����")]
    public Vector2 inputVector;

    [Header("�̵� �ӵ�")]
    public float moveSpeed;

    #region ������ ��ǲ �ý���
    /*
    void Update()
    {
        // (Raw : 1 �Ǵ� 0���� �� ������) ������ ��ȹ�� ���� ������ ����
        inputVector.x = Input.GetAxisRaw("Horizontal");
        inputVector.y = Input.GetAxisRaw("Vertical");
    }
    */
    #endregion

    void Awake()
    {
        scanner = GetComponent<Scanner>();
        // ��Ȱ��ȭ �Ǿ��ִ� ������Ʈ�� ������ �� ����
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

    // ���� ���������� �Ѿ�� ������ ȣ��
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
            // �ڽ� ������Ʈ 3�� ° �ͺ��� ��Ȱ��ȭ
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
            // �ε�ȣ�� ���̸� true
            sprite.flipX = inputVector.x < 0;
        }
    }

    private void MoveAnimation()
    {
        // ���� ���� ���� ũ�⸸ ������
        ani.SetFloat("Speed", inputVector.magnitude);
    }

    void MovePlayer()
    {
        #region ���� �̵� ����
        /*
        // 1. �� 
        rigid.AddForce(inputVector);

        // 2. �ӷ� ����
        rigid.velocity = inputVector;
        */

        #endregion
        // 3. ��ġ ��� �̵� (���� ��ġ���� ������ ��ġ)

        // ���⸸�� �������� ����ȭ
        // �밢�� �̵��� ��� ��Ÿ��� ��Ģ�� ���� 1���� ū ���� �ǹǷ� �밢�� �̵��� �� �������Ƿ� ����ȭ�� �ʿ��ϴ�.
        //Vector2 nextVector = inputVector.normalized * moveSpeed * Time.fixedDeltaTime;

        // ���ο� ��ǲ �ý��� ��Ű���� ����� ��� �ν����Ϳ��� ����ȭ��Ű�� ������ ��ũ��Ʈ ������ �� �ʿ䰡 ����.
        Vector2 nextVector = inputVector * moveSpeed * Time.fixedDeltaTime;

        rigid.MovePosition(rigid.position + nextVector);
    }

    // ��ǲ �ý��� ��Ű�� ���
    void OnMove(InputValue value)
    {
        inputVector = value.Get<Vector2>();
    }
}
