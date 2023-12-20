using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // ����� �������� ������� ���� 
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

        // 0���� ũ�ų� ������ ���Ÿ� ������ ó��
        if(penetration >= 0)
        {
            rigid.velocity = dir * 15f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� �ƴϰų� ���������� ����
        if(!collision.CompareTag("Enemy") || penetration == -100)
        { return; }

        // ������ ����
        penetration--;

        // ������ 0���� ������
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
