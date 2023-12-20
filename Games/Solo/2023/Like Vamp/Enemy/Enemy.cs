using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("�ִ� ü��")]
    public float maxHealth;
    [Header("���� ü��")]
    public float health;
    [Header("���� �ӵ�")]
    public float moveSpeed;
    [Header("Ÿ��")]
    public Rigidbody2D target;
    [Header("��Ÿ�� �ִϸ����� ��Ʈ�ѷ�")]
    public RuntimeAnimatorController[] controller;
        
    [SerializeField]
    [Header("������ٵ�")]
    Rigidbody2D rigid;        
    [SerializeField]
    [Header("��������Ʈ ������")]
    SpriteRenderer sprite;
    Collider2D col;

    // �ڷ�ƾ���� �޾� �� ����
    WaitForFixedUpdate wait;

    Animator ani;

    bool isLive;

    void Awake()
    {
        ani = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        wait = new WaitForFixedUpdate();
    }

    void OnEnable()
    {
        // Ȱ��ȭ �� Ÿ�� �ʱ�ȭ
        target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        col.enabled = true;        
        rigid.simulated = true;
        sprite.sortingOrder = 2;
        ani.SetBool("Dead", false);
        // �׾��� �ٽ� ��Ƴ� �� �ƽ� ü������ �ʱ�ȭ
        health = maxHealth;
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;

        ChaseTaret();
    }

    void LateUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;

        FlipOn();
    }

    void ChaseTaret()
    {
        // ���ʹ� �׻� �÷��̾ �����ϱ� ������ �˹��� ����
        // ���� �ִϸ��̼��� ���°� Hit���� �˻��Ͽ� ���Ͻ����ش�.
        if(!isLive || ani.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            return;
        }    

        Vector2 dir = target.position - rigid.position;
        Vector2 dest = dir.normalized * moveSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + dest);

        // �÷��̾�� ���Ͱ� �浹�� �� �ӷ¿� ���� ���Ͱ� �з����� ���� �����ϱ� ���� ���ν�Ƽ ���� 0���� �׻� �����Ѵ�.
        rigid.velocity = Vector2.zero;
    }

    void FlipOn()
    {
        if (!isLive)
        {
            return;
        }

        // Ÿ���� x���� ���� x������ ���� �� �ø�
        sprite.flipX = target.position.x < rigid.position.x;
    }

    public void Init(SpawnData spawnData)
    {
        // �ִϸ���Ʈ ��Ʈ�ѷ��� ������������ ��������Ʈ Ÿ������ ����
        // ��Ÿ�� �ִϸ����� ��Ʈ�ѷ��� Sprite�� �����Ǿ� �ִ�. 
        ani.runtimeAnimatorController = controller[spawnData.spriteType];
        moveSpeed = spawnData.moveSpeed;
        maxHealth = spawnData.health;
        health = spawnData.health;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Bullet") || !isLive)  // ���� �浹 �������� ������� ���� �۵�
        {
            return;
        }

        health -= collision.GetComponent<Bullet>().damage;

        StartCoroutine(KnockBack());

        // �ǰ�ó��
        if(health > 0)
        {
            ani.SetTrigger("Hit");
            AudioManager.instance.PlaySFX(AudioManager.SFX.Hit);
        }
        else
        {
            isLive = false;
            col.enabled = false;
            // ���� �ùķ��̼� ����
            rigid.simulated = false;
            sprite.sortingOrder = 1;    // �׾��� �� ���� ������ �ʵ��� ����
            ani.SetBool("Dead", true);
            GameManager.Instance.kill++;
            GameManager.Instance.GetExp();

            if(GameManager.Instance.isLive)
            {
                AudioManager.instance.PlaySFX(AudioManager.SFX.Dead);
            }
        }
    }

    IEnumerator KnockBack()
    {
        // �ϳ��� ���� �������� ��ٸ��� ����
        yield return wait;

        // �÷��̾� �ݴ�������� �о
        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 reverseDir = transform.position - playerPos;
        rigid.AddForce(reverseDir.normalized * 3, ForceMode2D.Impulse);
    }

    // �ִϸ��̼ǿ��� ȣ��
    void Dead()
    {        
        gameObject.SetActive(false);
    }
}
