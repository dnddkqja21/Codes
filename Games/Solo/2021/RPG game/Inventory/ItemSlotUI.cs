using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class ItemSlotUI : MonoBehaviour, 
    IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item; // ȹ���� ������
    public int itemCount;   // ����
    public Image itemImage; // �̹���

    public TextMeshProUGUI countText;   // ������ ī��Ʈ
        
    public GameObject CountImage;   // ������ �ִ� �������� ��� ī��Ʈ �̹���

    public LoadingBar loading;
    
    ActionController ac;    // ������ ����ϴ� �׼���Ʈ�ѷ�    

    Player_PF player;    

    Vector3 respawnPos = new Vector3(0, 0, -3.5f);

    NavMeshAgent playerNav;

    void Start()
    {
        // �׼� ��Ʈ�ѷ��� ã����.
        ac = FindObjectOfType<ActionController>();
        
        player = FindObjectOfType<Player_PF>();        

        playerNav = player.gameObject.GetComponent<NavMeshAgent>();
    }

    // ������ �̹��� ���� ����
    void SetColor(float _a)
    {
        Color color = itemImage.color;
        color.a = _a;
        itemImage.color = color;
    }

    // ������ ȹ��
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;

        if(item.itemType != Item.ItemType.Equipment)    // ��� �ƴ� ���
        {
            CountImage.SetActive(true);
            countText.text = itemCount.ToString();                        
        }
        else
        {
            countText.text = "0";
            CountImage.SetActive(false);
        }
        SetColor(1);
    }

    // ������ ���� ����
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        countText.text = itemCount.ToString();

        if(itemCount <= 0)
        {
            ClearSlot();
        }
    }

    // ���� �ʱ�ȭ
    public void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);
        countText.text = "0";
        CountImage.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // ��Ŭ�� �� 
        Debug.Log("�κ��丮 ��Ŭ��");
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                if (item.itemType == Item.ItemType.Equipment)   // ����� ��� 
                {
                    Debug.Log("���� �̱���");
                }
                else if(item.itemType == Item.ItemType.Quest)   // ����Ʈ�� ���
                {
                    Debug.Log("����");
                }
                else
                {
                    switch(item.itemName)
                    {
                        case "ü�� ����":
                            Debug.Log("ü�� ȸ��");
                            ac.PlayClips(4);
                            player.curHP += item.recoveryValue;
                            break;

                        case "���� ����":
                            ac.PlayClips(4);
                            player.curMP += item.recoveryValue;
                            break;

                        case "��ȯ �ֹ���":
                            Debug.Log("��ȯ");
                            loading.gameObject.SetActive(true);
                            playerNav.enabled = false;
                            player.transform.position = respawnPos;
                            playerNav.enabled = true;
                            break;

                        case "�巡�� �̻�":
                            playerNav.enabled = false;
                            player.transform.position = respawnPos;
                            playerNav.enabled = true;                            
                            break;
                    }
                    SetSlotCount(-1);   // �Ҹ� �������� ��� ī��Ʈ ����                    
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("�巡�� ����");
        if(item != null)
        {
            DragSlot.instance.dragSlot = this;  // �巡�� ���Կ� �ڽ�(�ش� ����)�� ����
            DragSlot.instance.DragSetImage(itemImage);  // ������ �̹����� �־��ش�.
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // �巡�� ������ �ʱ�ȭ
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;        
    }

    // �ٸ� ���� ������ �巡�װ� ������ ��� ȣ��
    public void OnDrop(PointerEventData eventData)
    {
        // �� ������ �巡���� ��� ü���� ������ ȣ������ ���ϱ� ����.
        if(DragSlot.instance.dragSlot != null)
        {
            ChangeSlot();
        }
    }

    void ChangeSlot()
    {
        // �巡�� �� ����� ������ ���� ������ �������� �ӽ� ������ ������.
        Item tmp = item;
        int tmpCount = itemCount;

        // �巡�� ���� ������ �������� �־� ��
        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        
        if(tmp != null)
        {
            // ����� �Ǵ� ���� �������� �ִٸ� ��ü
            DragSlot.instance.dragSlot.AddItem(tmp, tmpCount);
        }
        else
        {
            // �� �����̸� �巡�� ���� Ŭ����
            DragSlot.instance.dragSlot.ClearSlot();
        }
    }

    // �����Ͱ� ���� �ȿ� ������ ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            ac.ShowTooltip(item, transform.position);
        }
    }

    // ������ ��
    public void OnPointerExit(PointerEventData eventData)
    {
        ac.HideTooltip();
    }    
}
