using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName ="New Item/item")]

public class Item : ScriptableObject    // 게임 오브젝트에 붙지않아도 됨
{
    public string itemName; // 이름
    public Sprite itemImage;    // 이미지
    public GameObject itemPrefab;   // 아이템 프리팹
    public int value;   // 아이템 인덱스
    public ItemType itemType;   // 아이템 타입
    public float recoveryValue; // 회복 값
    public int gold;    // gold

    [TextArea]
    public string itemDesc; // 아이템 설명

    
    public enum ItemType
    {
        Equipment,  // 장비
        Used,   // 소모
        Quest,  // 퀘스트
        Gold        // gold 
    }
    
}
