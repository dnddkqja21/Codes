using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 무기는 데미지와 관통력을 지님 
    public float damage;
    public int penetration;

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int penetration, Vector3 dir)
    {
        this.damage = damage;
        this.penetration = penetration;        

        // 0보다 크거나 같으면 원거리 무기라고 처리
        if(penetration >= 0)
        {
            rigid.velocity = dir * 15f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 적이 아니거나 근접무기라면 리턴
        if(!collision.CompareTag("Enemy") || penetration == -100)
        { return; }

        // 관통은 빼줌
        penetration--;

        // 관통이 0보다 작으면
        if(penetration < 0) 
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // Area
        if (!collision.CompareTag("Area") || penetration == -100)
            return;

        gameObject.SetActive(false);
    }
}
