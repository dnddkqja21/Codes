using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryUI : MonoBehaviour
{
    public static bool inventoryActivated = false;  // �κ��丮 ������ �� ����
    
    public GameObject inventoryBG;  // ��׶��� �̹���
    
    public GameObject gridSetting;  // �׸��� ����

    public ItemSlotUI[] slots; // ���Թ迭

    ActionController player;

    public static InventoryUI instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        slots = gridSetting.GetComponentsInChildren<ItemSlotUI>();  // �׸��� ������ �迭�� �ڽ�������Ʈ�� ������ �������� ä��    
        player = FindObjectOfType<ActionController>();
    }

    void Update()
    {
        TryInventory();
    }

    // �κ��丮 ���ݱ� �õ�
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

    // ���Կ� ������ ä���
    public void AddSlotItem(Item _item, int _count = 1)
    {
        if(Item.ItemType.Equipment != _item.itemType)   // ��� �������� �ƴ� ��쿡�� �������� ��ħ
        {
            for(int i = 0; i < slots.Length; i++)
            {
                if(slots[i].item != null)   // �ش� ������ ���� �ƴϸ�
                {
                    if(slots[i].item.itemName == _item.itemName)    // ������ �̸� ��ġ�ϴٸ� �� �������� ī��Ʈ��ŭ ����
                    {
                        slots[i].SetSlotCount(_count);
                        return;
                    }            
                }
            }
        }

        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)   // �� ������ �ִٸ�
            {
                slots[i].AddItem(_item, _count);    // �������� �߰�
                return;
            }
        }
    }

    public int FindItem(string _itemName, int _count)   // ������ �̸��� ������ ������ �ش� ������ ����
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
