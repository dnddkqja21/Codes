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
        // awakeȣ�� �� ���� �Ŵ��� ����� ȣ��� ���� ���������� �� ����Ƿ� ȣ�� ������ �����Ͽ� ��ŸƮ���� ȣ���Ѵ�.
        // �÷��̾� �ڽ� ������Ʈ �� area������ �ڽ� �ݶ��̴� ������ ���� 
        area = GameManager.Instance.player.GetComponentInChildren<BoxCollider2D>();
        tileSize = area.size.x;
        //Debug.Log(tileSize);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // �±׸� ����
        // Area �±װ� �ƴ� ��� ���� ��, Area �±׿��� ����� ���� �Ʒ� �ڵ带 ����
        if(!collision.CompareTag("Area"))
        {
            return;
        }

        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 tilePos = transform.position;

        // ���� �ڵ�
        #region ��ǲ ���Ϳ� ���Ͽ� ���� �̵���ų �� ���� ��ǲ�� 0�̶�� ���� �����ڿ� ���� 1�� ������ ������ ���� �ǵ��� �ٸ� ���̵� ����
         
        //// x,y ������ ����ȭ
        //Vector3 playerDir = GameManager.Instance.player.inputVector;
        
        #endregion   
        
        // ���� �±׷� �б�
        // Area ������ ��� �� "Ground" �Ǵ� "Enemy"�Ŀ� ���� �ٸ� �������� �̵�
        switch (transform.tag)
        {
            case "Ground":
                // ��ǲ ���Ϳ� ���� ���̵��� �ƴ� �ܼ��� �÷��̾��� ��ġ�� �̿��Ѵ�.
                //// �� �� �Ÿ� (�÷��̾� ��ġ - Ÿ�� ��ġ)
                float disX = playerPos.x - tilePos.x;
                float disY = playerPos.y - tilePos.y;
                //// x���� 0���� ������ -1 �ƴϸ� 1
                float dirX = disX < 0 ? -1 : 1;
                float dirY = disY < 0 ? -1 : 1;
                // ���Ŀ� ���밪 ���
                disX = Mathf.Abs(disX);
                disY = Mathf.Abs(disY);

                // �÷��̾�� Ÿ�ϸ��� �Ÿ� ���̿��� x���� y�ຸ�� ũ�� �����̵�                
                if (disX > disY)
                {
                    // �̵��� ������ �̵� (x�࿡ x�� ���� ���� �� �� �Ÿ��� ������.
                    // Ÿ�� ����� 2ĭ ������ �о�� �ϱ� ������ 2�� ���Ѵ�.
                    transform.Translate(Vector3.right * dirX * tileSize * 2);
                }
                else if(disX < disY)
                {
                    transform.Translate(Vector3.up * dirY * tileSize * 2);
                }
                break;

            case "Enemy":
                // �ݶ��̴��� Ȱ��ȭ�� ���� (������ ��� ����ִ� ����)
                if(myCollider.enabled)
                {
                    // �÷��̾�� Ÿ���� �������� 2�踸ŭ �̵�, ������ �ο�
                    Vector3 dir = playerPos - tilePos;
                    Vector3 random = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    transform.Translate(random + dir * 2);
                }
                break;
        }
    }
}
