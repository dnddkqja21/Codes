using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryUI : MonoBehaviour
{
    public static bool inventoryActivated = false;  // 인벤토리 여닫을 불 변수
    
    public GameObject inventoryBG;  // 백그라운드 이미지
    
    public GameObject gridSetting;  // 그리드 세팅

    public ItemSlotUI[] slots; // 슬롯배열

    ActionController player;

    public static InventoryUI instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        slots = gridSetting.GetComponentsInChildren<ItemSlotUI>();  // 그리드 세팅의 배열을 자식컴포넌트로 가져와 슬롯으로 채움    
        player = FindObjectOfType<ActionController>();
    }

    void Update()
    {
        TryInventory();
    }

    // 인벤토리 여닫기 시도
    public void TryInventory()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if(inventoryActivated)
            {
                OpenInventory();
                player.PlayClips(2);
            }
            else
            {
                CloseInventory();
            }
        }
    }

    void OpenInventory()
    {
        inventoryBG.SetActive(true);
    }

    void CloseInventory()
    {
        inventoryBG.SetActive(false);
    }

    // 슬롯에 아이템 채우기
    public void AddSlotItem(Item _item, int _count = 1)
    {
        if(Item.ItemType.Equipment != _item.itemType)   // 장비 아이템이 아닐 경우에만 아이템을 겹침
        {
            for(int i = 0; i < slots.Length; i++)
            {
                if(slots[i].item != null)   // 해당 슬롯이 널이 아니면
                {
                    if(slots[i].item.itemName == _item.itemName)    // 아이템 이름 일치하다면 그 아이템의 카운트만큼 증가
                    {
                        slots[i].SetSlotCount(_count);
                        return;
                    }            
                }
            }
        }

        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)   // 빈 슬롯이 있다면
            {
                slots[i].AddItem(_item, _count);    // 아이템을 추가
                return;
            }
        }
    }

    public int FindItem(string _itemName, int _count)   // 아이템 이름과 수량이 맞으면 해당 슬롯을 리턴
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if(slots[i].item != null)
            {
                if(slots[i].item.itemName == _itemName && slots[i].itemCount == _count)
                {                    
                    return i;  
                }                          
            }
        }
        return -1;
    }

    public int FindItemCount(string _itemName)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                if(slots[i].item.itemName == _itemName)
                {
                    return slots[i].itemCount;
                }
            }
        }
        return 0;
    }    
}
