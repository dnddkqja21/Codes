using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName ="New Item/item")]

public class Item : ScriptableObject    // ���� ������Ʈ�� �����ʾƵ� ��
{
    public string itemName; // �̸�
    public Sprite itemImage;    // �̹���
    public GameObject itemPrefab;   // ������ ������
    public int value;   // ������ �ε���
    public ItemType itemType;   // ������ Ÿ��
    public float recoveryValue; // ȸ�� ��
    public int gold;    // gold

    [TextArea]
    public string itemDesc; // ������ ����

    
    public enum ItemType
    {
        Equipment,  // ���
        Used,   // �Ҹ�
        Quest,  // ����Ʈ
        Gold        // gold 
    }
    
}
