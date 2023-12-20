using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("최대 체력")]
    public float maxHealth;
    [Header("현재 체력")]
    public float health;
    [Header("추적 속도")]
    public float moveSpeed;
    [Header("타겟")]
    public Rigidbody2D target;
    [Header("런타임 애니메이터 컨트롤러")]
    public RuntimeAnimatorController[] controller;
        
    [SerializeField]
    [Header("리지드바디")]
    Rigidbody2D rigid;        
    [SerializeField]
    [Header("스프라이트 렌더러")]
    SpriteRenderer sprite;
    Collider2D col;

    // 코루틴에서 받아 쓸 변수
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
        // 활성화 시 타겟 초기화
        target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        col.enabled = true;        
        rigid.simulated = true;
        sprite.sortingOrder = 2;
        ani.SetBool("Dead", false);
        // 죽었다 다시 살아날 시 맥스 체력으로 초기화
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
        // 몬스터는 항상 플레이어를 추적하기 때문에 넉백을 위해
        // 현재 애니메이션의 상태가 Hit인지 검사하여 리턴시켜준다.
        if(!isLive || ani.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            return;
        }    

        Vector2 dir = target.position - rigid.position;
        Vector2 dest = dir.normalized * moveSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + dest);

        // 플레이어와 몬스터가 충돌할 때 속력에 의해 몬스터가 밀려나는 것을 방지하기 위해 벨로시티 값을 0으로 항상 고정한다.
        rigid.velocity = Vector2.zero;
    }

    void FlipOn()
    {
        if (!isLive)
        {
            return;
        }

        // 타겟의 x값이 나의 x값보다 작을 때 플립
        sprite.flipX = target.position.x < rigid.position.x;
    }

    public void Init(SpawnData spawnData)
    {
        // 애니메이트 컨트롤러를 스폰데이터의 스프라이트 타입으로 대입
        // 런타임 애니메이터 컨트롤러에 Sprite도 보관되어 있다. 
        ani.runtimeAnimatorController = controller[spawnData.spriteType];
        moveSpeed = spawnData.moveSpeed;
        maxHealth = spawnData.health;
        health = spawnData.health;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Bullet") || !isLive)  // 연속 충돌 막기위해 살아있을 때만 작동
        {
            return;
        }

        health -= collision.GetComponent<Bullet>().damage;

        StartCoroutine(KnockBack());

        // 피격처리
        if(health > 0)
        {
            ani.SetTrigger("Hit");
            AudioManager.instance.PlaySFX(AudioManager.SFX.Hit);
        }
        else
        {
            isLive = false;
            col.enabled = false;
            // 물리 시뮬레이션 유무
            rigid.simulated = false;
            sprite.sortingOrder = 1;    // 죽었을 시 몬스터 가리지 않도록 변경
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
        // 하나의 물리 프레임을 기다리는 변수
        yield return wait;

        // 플레이어 반대방향으로 밀어냄
        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 reverseDir = transform.position - playerPos;
        rigid.AddForce(reverseDir.normalized * 3, ForceMode2D.Impulse);
    }

    // 애니메이션에서 호출
    void Dead()
    {        
        gameObject.SetActive(false);
    }
}
